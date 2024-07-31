using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStoreMVC.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) { 
            _context    = context;

        }

        public Task<Product> AddProductAsync( Product product)
        {
            product.CreatedDate = DateTime.Now;
            _context.Products.Add(product);
            _context.SaveChanges();
            return Task.FromResult(product);
        }

        public Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task DeleteProductByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products.OrderByDescending(p => p.ProductId).ToListAsync();
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId );
            return product;

        }

        public Task<Product> UpdateProductAsync(Product product)
        {
            product.CreatedDate = DateTime.Now;
            _context.Products.Update(product);
            _context.SaveChanges();
            return Task.FromResult(product);
        }
    }
}
