using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace BaseInfraestructure.Persistence.Helpers
{
    public static class ConfigurationHelper
    {
        public static IMongoDatabase BuildMongoDatabase(string context, IConfiguration configuration, bool writeContext = true)
        {
            var operation = writeContext ? "ConnectionStringWrite": "ConnectionStringRead";
            var stringConnection = configuration[$"MongoDB:{context}:{operation}"];
            var database = configuration[$"MongoDB:{context}:Database"];
            var mongoClient = new MongoClient(stringConnection);

            return mongoClient.GetDatabase(database);
        }
    }
}