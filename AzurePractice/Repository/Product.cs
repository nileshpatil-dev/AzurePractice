using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;

namespace AzurePractice.Repository
{
    public class Product : TableEntity
    {
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string ImageSrc { get; set; }

    }

    public class ProductDTO
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string fileName { get; set; }
        public int Price { get; set; }
        public byte[] file { get; set;}

    }

}