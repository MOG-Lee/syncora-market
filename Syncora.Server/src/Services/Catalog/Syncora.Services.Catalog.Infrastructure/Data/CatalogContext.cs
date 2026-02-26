using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Syncora.Services.Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Catalog.Infrastructure.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }

    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IOptions<CatalogDatabaseSettings> settings)
        {
            //MongoClient manages its own connection pool
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            Products = database.GetCollection<Product>("Products");

            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
