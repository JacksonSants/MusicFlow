using MongoDB.Driver;
using MusicFlow.Model.Artists;
using MusicFlow.Model.User;
using MusicFlow.Model.Musics;

namespace MusicFlow.Data.ContextApp;

public class MongoContextApp
{
    private readonly IMongoDatabase _database;

    public MongoContextApp(IConfiguration configuration)
    {
        DotNetEnv.Env.Load();

        var user = Environment.GetEnvironmentVariable("MONGO_USER");
        var pass = Environment.GetEnvironmentVariable("MONGO_PASS");

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            throw new InvalidOperationException("MongoDB credentials are not set in environment variables.");
        }

        var connectionStringApp = configuration.GetConnectionString("MongoDb");

        var connectionString = connectionStringApp
            .Replace("{USER}", user)
            .Replace("{PASS}", pass);
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("MusicFlow");
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Artist> Artists => _database.GetCollection<Artist>("Artist");
    public IMongoCollection<MusicFlow.Model.Musics.Music> Musics => _database.GetCollection<MusicFlow.Model.Musics.Music>("Music");
}
