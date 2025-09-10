using MongoDB.Bson;
using MongoDB.Driver;
using MusicFlow.Data.ContextApp;

namespace MusicFlow.Data.Service.MusicService;

public class MusicService
{
    private readonly MongoContextApp _context;

    public MusicService(MongoContextApp context)
    {
        _context = context;
    }

    //public async Task<List<BsonDocument>> GetMusicsWithArtistAsync()
    //{
    //    var pipeline = _context.Musics.Aggregate()
    //        .Lookup(
    //            foreignCollection: _context.Artists,
    //            localField: "artistId",
    //            foreignField: "_id",
    //            @as: "artist"
    //        )
    //        .Unwind("artist");

    //    return await pipeline.ToListAsync();
    //}
}
