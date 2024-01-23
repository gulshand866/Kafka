using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.IRepo;
using Product.API.Models;

namespace Product.API.Repo
{
    public class ProductRepo : IProductRepo
    {

        private readonly AppDbContext _appDbContext;

        public ProductRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            return await _appDbContext.products.ToListAsync();
        }

        public async Task<Products> GetProduct(int id)
        {
            var product = await _appDbContext.products.FindAsync(id);
            if (product is null)
            {
                throw new Exception("Product not found.");
            }
            return product;

        }

        public async Task<IEnumerable<Products>> CreateProduct(Products product)
        {
            _appDbContext.products.Add(product);
            await _appDbContext.SaveChangesAsync();

            return await GetProducts();
        }

        public async Task<IEnumerable<Products>> UpdateProduct(Products product)
        {
            var productToUpdate = await _appDbContext.products.FindAsync(product.Id);
            if (productToUpdate is null)
            {
                throw new Exception("Product not found.");
            }

            productToUpdate.Name = product.Name;
            productToUpdate.Quantity = product.Quantity;
            productToUpdate.Price = product.Price;
            await _appDbContext.SaveChangesAsync();

            return await GetProducts();
        }

        public async Task<IEnumerable<Products>> DeleteProduct(int id)
        {
            var productToDelete = await _appDbContext.products.FindAsync(id);
            if (productToDelete is null)
            {
                throw new Exception("Product not found.");
            }

            _appDbContext.products.Remove(productToDelete);
            await _appDbContext.SaveChangesAsync();

            return await GetProducts();

        }
    }
}

