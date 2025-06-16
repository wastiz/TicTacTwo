using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }
        
        public (bool Success, string Message) CreateUser(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                return (false, "Username already exists");
            }

            _context.Users.Add(new User { Username = username, Password = password });
            _context.SaveChanges();
            return (true, "User created successfully");
        }
        
        public (bool Success, string Message) DeleteUser(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return (false, "User not found");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return (true, "User deleted successfully");
        }
        
        public (bool Success, string Message) UpdateUser(string userId, string newUsername, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
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
        
        public User GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }
        
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public string GetUserNameById(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId)?.Username;
        }

        public (bool, string, User?) Login (string username, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            
            if (existingUser == null)
            {
                return (false, "User not found!", null);
            }

            if (existingUser.Password != password)
            {
                return (false, "Wrong username/password!", null);
            }
            
            return (true, "Successfully logged in", existingUser);
        }
    }
}
