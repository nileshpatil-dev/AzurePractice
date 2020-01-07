using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzurePractice.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzurePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public async Task<List<Product>> GetAsync()
        {
            var productRepository = new ProductRepository();

            return await productRepository.GetProductList();
        }

        [HttpPost]
        public async Task AddProduct()
        {
            var file = Request.Form.Files[0];
            var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            var name = dict["name"];
            var category = dict["category"];
            var price = Int32.Parse(dict["price"]);


            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    var productRepository = new ProductRepository();
                    file.CopyTo(ms);

                    var productDTO = new ProductDTO
                    {
                        fileName = Guid.NewGuid().ToString() + file.FileName.Split(".")[1],
                        file = ms.ToArray(),
                        Category = category,
                        Price = price,
                        Name = name
                    };
                    await productRepository.Insert(productDTO);
                }
            }

        }

        [HttpPost]
        [Route("delete-product")]
        public async Task DeleteProduct([FromBody] Product product)
        {
            var productRepository = new ProductRepository();

            await productRepository.Delete(product);
        }
    }
}