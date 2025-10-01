using AutoMapper;
using CourseTech.Domain.Dto.CourseResult;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities.UserRelated;

namespace CourseTech.Application.Mapping;

/// <summary>
/// Настройка маппинга для сущности "Профиль пользователя" в определённые DTO.
/// </summary>
public class UserProfileMapping : Profile
{
    public UserProfileMapping()
    {
        CreateMap<UserProfile, CourseResultDto>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.User.Login));

        CreateMap<UserProfile, UserProfileDto>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.User.Login));

        CreateMap<UserProfile, UserAnalysDto>();
    }
}
