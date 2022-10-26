using Catalog.Api.Entities;
using Catalog.Api.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoDbContext(IOptions<DatabaseSettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
            var products = GetCollection<Product>("Products");
            CatalogContextSeed.SeedData(products);
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
