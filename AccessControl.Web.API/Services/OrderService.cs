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

        public async Task<List<Order>> GetOrdersOnlyAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _dbContext.Orders
                       .Include(o => o.OrderItems)
                       .ToListAsync();
        }

        public async Task<Order?> GetOrderOnlyByIdAsync(int OrderId)
        {
            return await _dbContext.Orders.FindAsync(OrderId);
        }

        public async Task<Order?> GetOrderByIdAsync(int OrderId)
        {
            return await _dbContext.Orders.Include(o => o.OrderItems)
                          .FirstOrDefaultAsync(o => o.OrderId == OrderId);
        }
        //public async Task<Order> CreateOrderAsync(Order order)
        //{
        //    if (order==null)
        //    {

        //        throw new ArgumentNullException("Invalid Order Object"); 
        //    }
        //    await _dbContext.Orders.AddAsync(order);
        //    await _dbContext.SaveChangesAsync();

        //    return order;
        //}
        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            decimal totalAmount = 0;

            foreach (var item in order.OrderItems)
            {
                var product = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found.");

                item.UnitPrice = product.Price;
                item.TotalPrice = product.Price * item.Quantity;
                item.CreatedBy = "System";
                item.ModifiedBy = "System";
              

                totalAmount += item.TotalPrice;
            }

            order.TotalAmount = totalAmount;
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Delivered";
            order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}";
            order.CreatedBy = "System";
            order.CreatedDate = DateTime.UtcNow;
            order.ModifiedBy = "System";
            order.ModifiedDate = DateTime.UtcNow;
            

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }
        public async Task<Order> UpdateOrderAsync(int OrderId, Order order)
        {
            //find the respective object from the database using id
            //equel the database value to incoming values 
            //save the changes to database
            var OrderToUpdate = await _dbContext.Orders.FindAsync(OrderId);
            if (OrderToUpdate == null)
            {
                throw new KeyNotFoundException("Order Not Found.");
            }
            OrderToUpdate.OrderId = order.OrderId;
            OrderToUpdate.OrderNumber = order.OrderNumber;
            OrderToUpdate.CustomerName = order.CustomerName;
            OrderToUpdate.CustomerEmail = order.CustomerEmail;
            OrderToUpdate.CustomerPhone = order.CustomerPhone;
            OrderToUpdate.TotalAmount = order.TotalAmount;
            OrderToUpdate.OrderDate = order.OrderDate;
            OrderToUpdate.Status = order.Status;
            OrderToUpdate.ModifiedBy = order.ModifiedBy;
            OrderToUpdate.ModifiedDate = DateTime.UtcNow;
            OrderToUpdate.IsActive = order.IsActive;

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

        public async Task<bool> DeleteOrderItemAsync(int orderItemId)
        {
            if (orderItemId<=0)
            {
                throw new ArgumentException("Invalid Order Item Id");
            }
            var orderItem = await _dbContext.OrderItems.FirstOrDefaultAsync(a => a.OrderItemId == orderItemId);
            if (orderItem==null)
            {
                return false;
            }
            int orderId=orderItem.OrderId;
            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
             
            var order= await _dbContext.Orders.Include(b=>b.OrderItems).FirstOrDefaultAsync(b=>b.OrderId==orderId);
            if (order!=null)
            {
                order.TotalAmount = order.OrderItems.Sum(x => x.TotalPrice);
                order.ModifiedBy = "System";
                order.ModifiedDate = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
            }
            return true;
        }
   
     public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
