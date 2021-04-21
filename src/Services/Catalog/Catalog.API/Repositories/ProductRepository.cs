﻿using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _db;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _db = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return
                await _db.
                    Products.
                        Find(x => true).
                        ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return
                await _db.
                    Products.
                        Find(x => x.Id == id).
                        FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(x => x.Name, name);

            return
               await _db.
                   Products.
                       Find(filter).
                       ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryNamme)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Category, categoryNamme);

            return
               await _db.
                   Products.
                       Find(filter).
                       ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _db.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult =
                await _db.
                    Products.
                        ReplaceOneAsync(
                            filter: x => x.Id == product.Id,
                            replacement: product
                        );

            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }


        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, id);

            var deleteResult =
                await _db.
                    Products.
                    DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

    }
}
