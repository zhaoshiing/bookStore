using BookStore.Model;
using BookStore.Services;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
namespace BookStore.Tests
{
    public class OrderServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 每个测试使用唯一的数据库
                .Options;

            _context = new ApplicationDbContext(options);
            var logger = new LoggerFactory().CreateLogger<OrderService>();
            _orderService = new OrderService(logger, _context);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrder()
        {
            var order = new Order
            {
                UserId = 1,
                BookId = 1,
                OrderDate = DateTime.UtcNow
            };

            var result = await _orderService.CreateOrderAsync(order);

            Assert.Equal(1, result.UserId);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            var order = new Order
            {
                UserId = 1,
                BookId = 1,
                OrderDate = DateTime.UtcNow
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var result = await _orderService.GetOrderByIdAsync(order.Id);

            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            var result = await _orderService.GetOrderByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
        {
            await _context.Orders.AddRangeAsync(
                new Order { UserId = 1, BookId = 1, OrderDate = DateTime.UtcNow },
                new Order { UserId = 2, BookId = 2, OrderDate = DateTime.UtcNow }
            );
            await _context.SaveChangesAsync();

            var result = await _orderService.GetAllOrdersAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder()
        {
            var order = new Order
            {
                UserId = 1,
                BookId = 1,
                OrderDate = DateTime.UtcNow
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            order.UserId = 2;
            var result = await _orderService.UpdateOrderAsync(order);

            Assert.True(result);
            var updatedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Equal(2, updatedOrder.UserId);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrder()
        {
            var order = new Order
            {
                UserId = 1,
                BookId = 1,
                OrderDate = DateTime.UtcNow
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var result = await _orderService.DeleteOrderAsync(order.Id);

            Assert.True(result);
            var deletedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Null(deletedOrder);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}