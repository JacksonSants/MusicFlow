using MongoDB.Driver;
using MusicFlow.Data.ContextApp;
using MusicFlow.Model.User;

namespace MusicFlow.Data.Service.UserService;


public class UserRepository : IUserRepository
{
    private readonly MongoContextApp _context;
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoCollection<User> users, MongoContextApp context)
    {
        _users = users;
        _context = context;
    }

    public async Task<User?> Authenticate(string email, string password)
    {
        return await _users.Find(user => user.Email == email && user.Password == password).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetAllUsers() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User?> GetUserById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            throw new ArgumentException("Invalid artist ID format", nameof(id));
        }
        return await _users.Find(u => u.Id == guidId).FirstOrDefaultAsync();
    }

    public async Task CreateUser(User user)
    {
        if (user.Id == Guid.Empty)
            user.Id = Guid.NewGuid();

        if (user.DateCreated == default)
            user.DateCreated = DateTime.UtcNow;

        await _users.InsertOneAsync(user);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        return await _users.Find(filter).AnyAsync();
    }

    public async Task UpdateUser(string id, User user)
    {
        if (Guid.TryParse(id, out var guidId))
        {
            await _users.ReplaceOneAsync(u => u.Id == guidId, user);
        }
    }

    public async Task<User> DeleteUserById(string id)
    {
        if (Guid.TryParse(id, out var guidId))
        {
            var user = await _users.FindOneAndDeleteAsync(u => u.Id == guidId);
            return user ?? throw new System.Exception("User not found");
        }
        throw new ArgumentException("Formato de ID inválido", nameof(id));
    }

}
