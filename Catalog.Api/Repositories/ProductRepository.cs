using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoDbContext _context;
        private readonly IMongoCollection<Product> _products;
        public ProductRepository(IMongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _products = _context.GetCollection<Product>("Products");
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _products
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<Product> GetProduct(string id)
        {
            return await _products
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

    }
}
