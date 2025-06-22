using DAL.Contracts;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }
        
        public async Task<(bool Success, string Message, User? User)> CreateUser(UserRegister dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return (false, "Username already exists", null);
            }

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return (false, "Email already exists", null);
            }

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return (true, "User created successfully", newUser);
        }

        
        public async Task<(bool Success, string Message)> DeleteUser(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return (false, "User not found");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return (true, "User deleted successfully");
        }
        
        public async Task<(bool Success, string Message)> UpdateUser(string userId, string newUsername, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return (false, "User not found");
            }

            if (_context.Users.Any(u => u.Username == newUsername && u.Id != userId))
            {
                return (false, "Username already exists");
            }

            user.Username = newUsername;
            user.Password = newPassword;
            _context.SaveChanges();
            return (true, "User updated successfully");
        }
        
        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<string> GetUserNameById(string userId)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return result.Username;
        }

        public async Task<(bool Success, string Message, User? User)> CheckPassword(UserLogin dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

            if (existingUser == null)
            {
                return (false, "User not found!", null);
            }

            if (existingUser.Password != dto.Password)
            {
                return (false, "Wrong username or password!", null);
            }

            return (true, "Successfully logged in", existingUser);
        }

    }
}
