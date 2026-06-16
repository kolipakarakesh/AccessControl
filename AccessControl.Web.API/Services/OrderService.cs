using AccessControl.Web.API.DBConfiguration;
using AccessControl.Web.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Web.API.Services
{
    public class OrderService : IOrderService,IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext= dbContext;
        }
      public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order==null)
            {

                throw new ArgumentNullException("Invalid Order Object"); 
            }
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        public async Task<bool> DeleteOrderAsync(int OrderId)
        {
            if (OrderId<=0)
            {
                throw new ArgumentException("Invalid Order Id");
            }
            var order=await _dbContext.Orders.FindAsync(OrderId);
            if (order==null)
            {
                return false;
            }
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }

       

        public async Task<Order?> GetOrderByIdAsync(int OrderId)
        {
            // return await _dbContext.Orders.FindAsync(OrderId);
            return await _dbContext.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.OrderId == OrderId);
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            //return await _dbContext.Orders.ToListAsync();
            return await _dbContext.Orders
                       .Include(o => o.OrderItems)
                       .ToListAsync();
        }

        public async Task<Order> UpdateOrderAsync(int OrderId, Order order)
        {
            //find the respective object from the database using id
            //equel the database value to incoming values 
            //save the changes to database
           var OrderToUpdate= await _dbContext.Orders.FindAsync(OrderId);
            if (OrderToUpdate == null)
            {
                throw new KeyNotFoundException("Order Not Found.");
            }
            OrderToUpdate.OrderId = order.OrderId;
            OrderToUpdate.OrderNumber= order.OrderNumber;
            OrderToUpdate.CustomerName= order.CustomerName;
            OrderToUpdate.CustomerEmail= order.CustomerEmail;
            OrderToUpdate.CustomerPhone= order.CustomerPhone;
            OrderToUpdate.TotalAmount= order.TotalAmount;
            OrderToUpdate.OrderDate = order.OrderDate;
            OrderToUpdate.Status= order.Status;
            OrderToUpdate.ModifiedBy= order.ModifiedBy;
            OrderToUpdate.ModifiedDate = DateTime.UtcNow;
            OrderToUpdate.IsActive = order.IsActive;

            await _dbContext.SaveChangesAsync();
            return order;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
