using Product.API.Models;

namespace Product.API.IRepo
{
    public interface IProductRepo
    {
        public Task<IEnumerable<Products>> GetProducts();
        public Task<Products> GetProduct(int id);
        public Task<IEnumerable<Products>> CreateProduct(Products product);
        public Task<IEnumerable<Products>> UpdateProduct(Products product);
        public Task<IEnumerable<Products>> DeleteProduct(int id);
    }
}
