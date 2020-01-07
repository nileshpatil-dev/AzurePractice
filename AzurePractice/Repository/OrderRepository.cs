using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzurePractice.Repository
{
    public class OrderRepository
    {

        private CloudStorageAccount GetStorageAccount()
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials("nileshonlineshop", "aJDv4RHrFS/Y+bQdoO9/BfgwokBpNbTvviP75mM09qMtzwITvwXPOk/pNeNSpLM6TESCIZZJZilw8Td/t3SERQ=="), true
                );
            return storageAccount;
        }

        private async Task<CloudQueue> GetCloudQueueAsync()
        {
            var storageAccount = GetStorageAccount();
            var cloudQueueClient = storageAccount.CreateCloudQueueClient();

            var cloudQueue = cloudQueueClient.GetQueueReference("orders");

            await cloudQueue.CreateIfNotExistsAsync();

            return cloudQueue;
        }

        public async Task PutOrder(Order order)
        {
            var cloudQueue = await GetCloudQueueAsync();

            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(order));

            await cloudQueue.AddMessageAsync(message);
        }

    }

    public class Order
    {
        public string ProductId { get; set; }
        public int Price { get; set; }
        public int CustomerId { get; set; }
        public int Qty { get; set; }
    }
}
