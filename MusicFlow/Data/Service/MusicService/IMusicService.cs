using MongoDB.Bson;

namespace MusicFlow.Data.Service.MusicService;

public interface IMusicService
{
    Task<List<BsonDocument>> GetMusicsWithArtistAsync();
}
