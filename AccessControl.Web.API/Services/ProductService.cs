using AccessControl.Web.API.DBConfiguration;
using AccessControl.Web.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Web.API.Services
{
    public class ProductService : IProductService, IDisposable
    {
        private readonly ApplicationDbContext _dbcontext;
        public ProductService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("Invalid product object");
            }

            await _dbcontext.Products.AddAsync(product);
            await _dbcontext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid product ID.");
            }
            var product = await _dbcontext.Products.FindAsync(productId);

            if (product == null)
            {
                return false;
            }
            _dbcontext.Products.Remove(product);
            await _dbcontext.SaveChangesAsync();
            return true;
        }



        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _dbcontext.Products.FindAsync(productId);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            //select * from products where isactive = 1 order by productid desc
            return await _dbcontext.Products.ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(int productId, Product product)
        {
            //find the respective object from the database using id
            //equel the database value to incoming values 
            //save the changes to database


            var productToUpdate = await _dbcontext.Products.FindAsync(productId);

            if (productToUpdate == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            productToUpdate.ProductName = product.ProductName;
            productToUpdate.ProductCode = product.ProductCode;
            productToUpdate.Price = product.Price;
            productToUpdate.StockQuantity = product.StockQuantity;
            productToUpdate.ModifiedBy = product.ModifiedBy;
            productToUpdate.ModifiedDate = DateTime.UtcNow;
            productToUpdate.IsActive = product.IsActive;

            await _dbcontext.SaveChangesAsync();
            return product;
        }
        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}
