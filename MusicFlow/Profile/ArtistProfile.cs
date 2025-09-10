using AutoMapper;
using MusicFlow.Data.Dto.Artist;
using MusicFlow.Model.Artists;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<CreateArtistDto, Artist>();
        CreateMap<UpdateArtistDto, Artist>();
    }
}
