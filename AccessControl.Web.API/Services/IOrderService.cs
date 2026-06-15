using AccessControl.Web.API.Models;

namespace AccessControl.Web.API.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int OrderId);
        Task<Order>CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(int OrderId,Order order);
        Task<bool>DeleteOrderAsync(int OrderId);
    }
}
