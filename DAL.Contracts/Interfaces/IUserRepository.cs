using DAL.Contracts.DTO;
using DAL.DTO;

namespace DAL.Contracts.Interfaces
{
    public interface IUserRepository
    {
        Task<Response<User>> CreateUser(UserRegister dto);
        Task<Response> DeleteUser(string userId);
        Task<Response> UpdateUser(string userId, string newUsername, string newPassword);
        Task<Response<User>> GetUserById(string userId);
        Task<Response<List<User>>> GetAllUsers();
        Task<Response<string>> GetUserNameById(string userId);
        Task<Response<User>> CheckPassword(UserLogin dto);
    }
}