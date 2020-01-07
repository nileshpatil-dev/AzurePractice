using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzurePractice.Repository
{
    public class ProductRepository
    {

        private CloudStorageAccount GetStorageAccount()
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials("nileshonlineshop", "aJDv4RHrFS/Y+bQdoO9/BfgwokBpNbTvviP75mM09qMtzwITvwXPOk/pNeNSpLM6TESCIZZJZilw8Td/t3SERQ=="), true
                );
            return storageAccount;
        }

        private async Task<CloudTable> GetTableAsync()
        {
            var storageAccount = GetStorageAccount();
            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("products");

            await table.CreateIfNotExistsAsync();

            return table;
        }


        private async Task<CloudBlobContainer> GetBlobAsync()
        {
            var storageAccount = GetStorageAccount();
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("product");

            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            await container.CreateIfNotExistsAsync();

            return container;
        }

        private async Task<string> UploadImageAsync(Product product, byte[] stream, string fileName)
        {
            var container = await GetBlobAsync();
            var cloudBlockBlob = container.GetBlockBlobReference(fileName.ToLower());
            await cloudBlockBlob.UploadFromByteArrayAsync(stream, 0, stream.Length -1);
            return cloudBlockBlob.Uri.ToString();
        }

        private async Task DeleteImageAsync(Product product) 
        {
            var container = await GetBlobAsync();
            var cloudBlockBlob = container.GetBlockBlobReference(product.RowKey.ToLower() + ".jpg");
            await cloudBlockBlob.DeleteIfExistsAsync();
        }

        public async Task Insert(ProductDTO item)
        {
            var product = new Product
            {
                PartitionKey = item.Category,
                RowKey = Guid.NewGuid().ToString(),
                ProductName = item.Name,
                Price = item.Price
            };
            product.ImageSrc = await UploadImageAsync(product, item.file, item.fileName);
            var table = await GetTableAsync();

            var operation = TableOperation.Insert(product);

            await table.ExecuteAsync(operation);
        }

        public async Task<List<Product>> GetProductList()
        {
            var table = await GetTableAsync();

            TableContinuationToken token = null;

            var entities = new List<Product>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(new TableQuery<Product>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        public async Task Delete(Product product)
        {
            var table = await GetTableAsync();

            var operation = TableOperation.Delete(GetProduct(product.PartitionKey, product.RowKey).Result);

            await DeleteImageAsync(product);

            await table.ExecuteAsync(operation);
        }

        public async Task<Product> GetProduct(string pKey, string rKey)
        {
            var table = await GetTableAsync();

            var operation = TableOperation.Retrieve<Product>(pKey, rKey);

            var tableResult = await table.ExecuteAsync(operation);
            var product = (Product)tableResult.Result;

            return product;
        }

    }
}
