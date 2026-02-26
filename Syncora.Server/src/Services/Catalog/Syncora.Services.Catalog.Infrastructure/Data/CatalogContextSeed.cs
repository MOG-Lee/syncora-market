using MongoDB.Driver;
using Syncora.Services.Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Catalog.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();

            if (!existProduct)
            {
                productCollection.InsertOne(new Product
                {
                    Name = "test",
                    Description = "test",
                    ImageFileName = "test.png",
                    Price = 1.00M,
                    Category = "test",
                    Attributes = new Dictionary<string, string>
                    {
                        { "Color", "Red" },
                        { "RAM", "16" }
                    },
                    Comments = new List<ProductComment>
                    {
                        new ProductComment
                        {
                            UserName = "test",
                            Comment = "test",
                            Rating = 5
                        }
                    }
                });
            }
        }
    }
}
