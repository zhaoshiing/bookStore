using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _logger.LogInformation("Creating user with username {Username}", user.Username);

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User created with ID {UserId}", user.Id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with username {Username}", user.Username);
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    return null;
                }

                _logger.LogInformation("User with ID {UserId} fetched successfully", id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with ID {UserId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users");

            try
            {
                var users = await _context.Users.ToListAsync();
                _logger.LogInformation("{Count} users fetched successfully", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _logger.LogInformation("Updating user with ID {UserId}", user.Id);

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID {UserId} updated successfully", user.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", user.Id);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            _logger.LogInformation("Deleting user with ID {UserId}", id);

            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    return false;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID {UserId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                throw;
            }
        }
        public User Authenticate(string userName,string usePwd)
        {
            _logger.LogInformation($"Authenticate user with userName {userName} and password {usePwd}");
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Username == userName && x.Password == usePwd);
                if (user == null)
                {
                    _logger.LogInformation($"cannot find user with userName {userName} and password {usePwd}");
                    return null; // Return null if the user is not found
                }
                _logger.LogInformation("Authenticate successfully with userName {userName} and password {usePwd}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Authenticate user  with userName {userName} and password {usePwd}");
                throw;
            }
        }
    }
}


