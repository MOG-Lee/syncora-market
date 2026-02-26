using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Syncora.Services.Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Catalog.Infrastructure.Data
{
    public static class MongoDbConfiguration
    {
        public static void Configure()
        {
            // Tell MongoDB to use the universal standard for Guids globally.
            try
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            }
            catch (BsonSerializationException ex)
            {
                // Serializer is already registered, do nothing.
            }

            // Check to prevent registering the map multiple times
            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
            {
                BsonClassMap.RegisterClassMap<Product>(map =>
                {
                    map.AutoMap(); // Automatically maps all other properties

                    // Configure the Id to be a Guid and act as the MongoDB _id
                    map.MapIdMember(x => x.Id)
                       .SetIdGenerator(CombGuidGenerator.Instance); // Generates sequential Guids for better DB performance

                    // Example of how to rename an element
                    map.MapMember(x => x.Name).SetElementName("Name");
                });
            }
        }
    }
}
