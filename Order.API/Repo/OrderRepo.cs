using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.IRepo;
using Order.API.Models;

namespace Order.API.Repo
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _appDbContext;

        public OrderRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Orders>> GetOrders()
        {
            return await _appDbContext.orders.ToListAsync();
        }

        public async Task<Orders> GetOrder(int id)
        {
            var order = await _appDbContext.orders.FindAsync(id);
            if (order is null)
            {
                throw new Exception("Order not found.");
            }
            return order;
        }


        public async Task<IEnumerable<Orders>> CreateOrder(Orders order)
        {
            _appDbContext.orders.Add(order);
            await _appDbContext.SaveChangesAsync();

            return (await GetOrders());
        }

        public async Task<IEnumerable<Orders>> DeleteOrder(int id)
        {
            var orderToDelete = await _appDbContext.orders.FindAsync(id);
            if (orderToDelete is null)
            {
                throw new Exception("Product not found.");
            }

            _appDbContext.orders.Remove(orderToDelete);
            await _appDbContext.SaveChangesAsync();

            return (await GetOrders());
        }




    }
}
