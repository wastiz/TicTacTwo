using DAL.Contracts.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.UserDtos;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response<User>> CreateUser(UserRegister dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return Response<User>.Fail("User with this username already exists");
            }

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return Response<User>.Fail("Email already exists");
            }

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Response<User>.Ok(newUser, "User created successfully");
        }


        
        public async Task<Response> DeleteUser(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new Response() { Success = false, Message = "User not found" };
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return new Response() { Success = true, Message = "User deleted successfully" };
        }
        
        public async Task<Response> UpdateUser(string userId, string newUsername, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new Response() { Success = false, Message = "User not found" };
            }

            if (_context.Users.Any(u => u.Username == newUsername && u.Id != userId))
            {
                return new Response() { Success = false, Message = "User with this username exists" };
            }

            user.Username = newUsername;
            user.Password = newPassword;
            _context.SaveChanges();
            return new Response() { Success = true, Message = "User updated successfully" };
        }
        
        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<string?> GetUsernameById(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Username;
        }

        public async Task<Response<User>> CheckPassword(UserLogin dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

            if (existingUser == null)
            {
                return Response<User>.Fail("No user found");
            }

            if (existingUser.Password != dto.Password)
            {
                return Response<User>.Fail("Passwords do not match");
            }

            return Response<User>.Ok(existingUser, "Passwords match");
        }

    }
}
