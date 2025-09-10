using MongoDB.Bson;
using MongoDB.Driver;
using MusicFlow.Data.ContextApp;
using MusicFlow.Data.Dto.ArtistWithMusic;
using MusicFlow.Model.Artists;
using MusicFlow.Model.User;

namespace MusicFlow.Data.Service.ArtistService;

public class ArtistService : IArtistService
{
    private readonly MongoContextApp _context;
    private readonly IMongoCollection<Artist> _artist;

    public ArtistService(MongoContextApp context, IMongoCollection<Artist> artist)
    {
        _context = context;
        _artist = artist;
    }

    public async Task<List<ArtistWithMusicsDto>> GetArtistsWithMusicsAsync()
    {
        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Music" },
                { "localField", "_id" },
                { "foreignField", "artistId" },
                { "as", "musics" }
            }),
            new BsonDocument("$project", new BsonDocument
            {
                { "name", 1 },
                { "biography", 1 },
                { "profileImage", 1 },
                { "country", 1 },
                { "genre", 1 },
                { "followers", 1 },
                { "verified", 1 },
                { "dateCreated", 1 },
                { "musics", 1 } 
            })
        };

        var documents = await _context.Artists.Aggregate<BsonDocument>(pipeline).ToListAsync();

        // Conversão manual para DTO
        var result = documents.Select(doc =>
        {
            var dto = new ArtistWithMusicsDto
            {
                Id = doc.GetValue("_id").AsGuid,
                Name = doc.GetValue("name").AsString,
                Biography = doc.GetValue("biography").AsString,
                ProfileImage = doc.GetValue("profileImage", BsonNull.Value).IsBsonNull ? null : doc["profileImage"].AsString,
                Country = doc.GetValue("country").AsString,
                Genre = doc["genre"].AsBsonArray.Select(g => g.AsString).ToList(),
                Followers = doc["followers"].AsInt32,
                Verified = doc["verified"].AsBoolean,
                DateCreated = doc["dateCreated"].ToUniversalTime(),
                Musics = doc["musics"].AsBsonArray.Select(m => new MusicDto
                {
                    Title = m["title"].AsString,
                    Genre = m["genre"].AsString
                }).ToList()
            };

            return dto;
        }).ToList();

        return result;
    }



    public async Task<Artist> CreateArtist(Artist artist)
    {
        artist.Id = Guid.NewGuid();
        artist.Followers = 0;
        artist.Verified = false;
        artist.DateCreated = DateTime.UtcNow;

        await _artist.InsertOneAsync(artist);
        return artist;
    }

    public async Task<List<Artist>> GetAllArtists()
    {
        return await _artist.Find(_ => true).ToListAsync();
    }

    public async Task<Artist?> GetArtistById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            throw new ArgumentException("Invalid artist ID format", nameof(id));
        }
        return await _artist.Find(u => u.Id == guidId).FirstOrDefaultAsync();
    }

    public async Task UpdateArtist (string Id, Artist artist)
    {
        if(Guid.TryParse(Id, out var guidId))
        {
            await _artist.ReplaceOneAsync(u => u.Id == guidId, artist);
        }
    }
}
