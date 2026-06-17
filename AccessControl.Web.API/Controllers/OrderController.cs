using AccessControl.Web.API.Helpers;
using AccessControl.Web.API.Models;
using AccessControl.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Web.API.Controllers
{
    [Route("api/[controller]")]
     [ApiController]
    public class OrderController:ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger,IOrderService orderService)
        {
          _logger = logger;
          _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders() 
        {
            try
            {
               var orders= await _orderService.GetOrdersAsync();
                _logger.LogInformation("Retrieved {Count} Orders",orders.Count);
                return Ok(orders);

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Internal Server Error : { ex.Message}");
                return StatusCode(500,$"Internal Server Error : {ex.Message}");
            }
        }

        [HttpGet("OrdersOnly")]
        public async Task<IActionResult> GetAllOrdersOnly()
        {
            try
            {
                var orders = await _orderService.GetOrdersOnlyAsync();
                _logger.LogInformation("Retrieved {Count} Orders", orders.Count);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error : {ex.Message}");
                return StatusCode(500, $"Internal Server Error : {ex.Message}");
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Order ID.");
                }
                var order = await _orderService.GetOrderByIdAsync(id);
                _logger.LogInformation("Retrieved Order with Id {Count}", id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex) 
            {
            _logger.LogError($"Internal Server Error {ex.Message}");
            return StatusCode(500,$"Internal Server Error {ex.Message}");
            }
        }

        [HttpGet]
        [Route("OrdersOnly/{id}")]
        public async Task<IActionResult>GetOrderOnlyById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Order ID.");
                }
                var order = await _orderService.GetOrderOnlyByIdAsync(id);
                _logger.LogInformation("Retrieved Order with Id {Count}", id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex) {
                _logger.LogError($"Internal Server Error {ex.Message}");
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }
        }

        [HttpPost]
        [AccessControlAutherise]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest("Order object is null.");
                }
                var createdOrder = await _orderService.CreateOrderAsync(order);
                _logger.LogInformation("Created order with ID {OrderId}", createdOrder.OrderId);
                return Ok(createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("{id}")]
        [AccessControlAutherise]
        public async Task<IActionResult> UpdateOrder(int id,Order order)
        {
            try
            {
                if (id<=0 || order==null)
                {
                    return BadRequest("Invalid order Id or order object is null.");
                }
                var updatedOrder= await _orderService.UpdateOrderAsync(id, order);
                if (updatedOrder==null)
                {
                    return NotFound();
                }
                _logger.LogInformation("Updated order with ID {OrderId}",id);
                return Ok(updatedOrder);
            }
            catch(Exception ex) 
            {
                _logger.LogError($"Internal Server error : {ex.Message}");
                return StatusCode(500, $"Internal server error : {ex.Message}");
            }
        }
        [HttpDelete]
        [Route("{id}")]
        [AccessControlAutherise]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid order ID.");
                }
                var deleted = await _orderService.DeleteOrderAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                _logger.LogInformation("Deleted order with ID {OrderId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("orderitem/orderItemId")]
        [AccessControlAutherise]
        public async Task<IActionResult>DeleteOrderItem(int orderItemId)
        {
           var result= await _orderService.DeleteOrderItemAsync(orderItemId);
            if (!result)
            {
                return NotFound("Order Item Not Found.");
            }
            return Ok("Order Item Deleted Successfully.");
        }
    

    }
}
