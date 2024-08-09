using BookStore.Model;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ILogger<OrderService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _logger.LogInformation("Creating order for user {UserId}", order.UserId);
            if (AnomalyDetectionService.IsFraudulentOrder(order))
            {
                throw new Exception("Suspicious order detected!");
            }
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order created with ID {OrderId}", order.Id);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for user {UserId}", order.UserId);
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            _logger.LogInformation("Fetching order with ID {OrderId}", id);

            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", id);
                    return null;
                }

                _logger.LogInformation("Order with ID {OrderId} fetched successfully", id);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order with ID {OrderId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            _logger.LogInformation("Fetching all orders");

            try
            {
                var orders = await _context.Orders.ToListAsync();
                _logger.LogInformation("{Count} orders fetched successfully", orders.Count);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all orders");
                throw;
            }
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _logger.LogInformation("Updating order with ID {OrderId}", order.Id);

            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order with ID {OrderId} updated successfully", order.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order with ID {OrderId}", order.Id);
                throw;
            }
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            _logger.LogInformation("Deleting order with ID {OrderId}", id);

            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", id);
                    return false;
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Order with ID {OrderId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order with ID {OrderId}", id);
                throw;
            }
        }
    }
}

