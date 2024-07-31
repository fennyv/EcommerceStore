using EcommerceStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreMVC.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> AddProductAsync(Product product);

        Task DeleteProductAsync(Product product);

        Task DeleteProductByIdAsync(int productId);

        Task<Product> UpdateProductAsync(Product product);



        Task<Product?> GetProductByIdAsync(int productId);


    }
}
