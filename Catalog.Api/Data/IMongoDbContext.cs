using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
