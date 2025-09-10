using AutoMapper;
using MusicFlow.Data.Dto.CreateUserDto;
using MusicFlow.Data.Dto.User;
using MusicFlow.Model.User;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<User, CreateUserDto>();
        CreateMap<User, UpdateUserDto>();
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore());

    }
}
