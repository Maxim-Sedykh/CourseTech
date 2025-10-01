using AutoMapper;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities.UserRelated;

namespace CourseTech.Application.Mapping;

/// <summary>
/// Настройка маппинга для сущности "Пользователь" в определённые DTO.
/// </summary>
public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name)))
            .ForMember(dest => dest.IsExamCompleted, opt => opt.MapFrom(src => src.UserProfile.IsExamCompleted))
            .ForMember(dest => dest.IsEditAble, opt => opt.MapFrom(src => src.UserProfile.IsEditAble));

        CreateMap<User, UpdateUserDto>()
            .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.UserProfile.Name))
            .ForPath(dest => dest.Surname, opt => opt.MapFrom(src => src.UserProfile.Surname))
            .ForPath(dest => dest.IsEditAble, opt => opt.MapFrom(src => src.UserProfile.IsEditAble))
            .ForMember(d => d.Id, (options) => options.Ignore());

        CreateMap<UpdateUserDto, User>()
            .ForPath(dest => dest.UserProfile.Name, opt => opt.MapFrom(src => src.UserName))
            .ForPath(dest => dest.UserProfile.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForPath(dest => dest.UserProfile.IsEditAble, opt => opt.MapFrom(src => src.IsEditAble));
    }
}
