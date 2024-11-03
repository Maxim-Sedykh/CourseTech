using AutoMapper;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Mapping
{
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
}
