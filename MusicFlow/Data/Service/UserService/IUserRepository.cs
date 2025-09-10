using MusicFlow.Model.User;

namespace MusicFlow.Data.Service.UserService;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserById(string id);
    Task CreateUser(User user);
    Task<bool> EmailExistsAsync(string email);
    Task<User?> Authenticate(string email, string password);
    Task UpdateUser(string id, User user);
    Task<User> DeleteUserById(string id);

}
