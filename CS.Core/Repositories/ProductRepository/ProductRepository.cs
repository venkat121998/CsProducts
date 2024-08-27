﻿using CS.Core.ApplicationContext;
using CS.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CS.Core.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateStockAsync(int id, int quantity, bool isIncrement)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                if (isIncrement)
                {
                    product.StockAvailable += quantity;
                }
                else
                {
                    product.StockAvailable = Math.Max(0, product.StockAvailable - quantity);
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
