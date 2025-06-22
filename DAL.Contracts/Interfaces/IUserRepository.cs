using System.Collections.Generic;
using DAL.DTO;

namespace DAL.Contracts
{
    public interface IUserRepository
    {
        Task<(bool Success, string Message, User? User)> CreateUser(UserRegister dto);
        Task<(bool Success, string Message)> DeleteUser(string userId);
        Task<(bool Success, string Message)> UpdateUser(string userId, string newUsername, string newPassword);
        Task<User?> GetUserById(string userId);
        Task<List<User>> GetAllUsers();
        Task<string?> GetUserNameById(string userId);
        Task<(bool Success, string Message, User? User)> CheckPassword(UserLogin dto);
    }
}