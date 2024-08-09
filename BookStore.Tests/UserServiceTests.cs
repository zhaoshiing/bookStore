using BookStore.Model;
using BookStore.Services;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System;
namespace BookStore.Tests
{
    public class UserServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 每个测试使用唯一的数据库
                .Options;

            _context = new ApplicationDbContext(options);
            var logger = new LoggerFactory().CreateLogger<UserService>();
            _userService = new UserService(logger, _context);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "securepassword"
            };

            var result = await _userService.CreateUserAsync(user);

            Assert.Equal("testuser", result.Username);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "securepassword"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var result = await _userService.GetUserByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var result = await _userService.GetUserByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            await _context.Users.AddRangeAsync(
                new User { Username = "user1", Email = "user1@example.com", Password = "password1" },
                new User { Username = "user2", Email = "user2@example.com", Password = "password2" }
            );
            await _context.SaveChangesAsync();

            var result = await _userService.GetAllUsersAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "securepassword"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user.Username = "updateduser";
            var result = await _userService.UpdateUserAsync(user);

            Assert.True(result);
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.Equal("updateduser", updatedUser.Username);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "securepassword"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var result = await _userService.DeleteUserAsync(user.Id);

            Assert.True(result);
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}