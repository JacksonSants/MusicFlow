using MongoDB.Bson;
using MusicFlow.Data.Dto.Artist;
using MusicFlow.Data.Dto.ArtistWithMusic;
using MusicFlow.Model.Artists;
using MusicFlow.Model.User;

namespace MusicFlow.Data.Service.ArtistService;

public interface IArtistService
{
    Task<List<ArtistWithMusicsDto>> GetArtistsWithMusicsAsync();
    Task<Artist> CreateArtist(Artist artist);
    Task<Artist?> GetArtistById(string id);
    Task UpdateArtist(string Id, Artist artist);
    Task<List<Artist>> GetAllArtists();
}
