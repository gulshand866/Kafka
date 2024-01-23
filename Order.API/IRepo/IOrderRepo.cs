using Order.API.Models;

namespace Order.API.IRepo
{
    public interface IOrderRepo
    {
        public Task<IEnumerable<Orders>> GetOrders();
        public Task<Orders> GetOrder(int id);
        public Task<IEnumerable<Orders>> CreateOrder(Orders order);
        public Task<IEnumerable<Orders>> DeleteOrder(int id);
    }
}
