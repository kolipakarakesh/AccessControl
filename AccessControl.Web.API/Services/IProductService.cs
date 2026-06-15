using AccessControl.Web.API.Models;

namespace AccessControl.Web.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int productId, Product product);
        Task<bool> DeleteProductAsync(int productId);
    }
}
