using AutoMapper;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name)))
                .ForMember(dest => dest.IsExamCompleted, opt => opt.MapFrom(src => src.UserProfile.IsExamCompleted))
                .ForMember(dest => dest.IsEditAble, opt => opt.MapFrom(src => src.UserProfile.IsEditAble));

            CreateMap<User, UpdateUserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.Name)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserProfile.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.UserProfile.Surname))
                .ForMember(dest => dest.IsEditAble, opt => opt.MapFrom(src => src.UserProfile.IsEditAble))
                .ReverseMap();
        }
    }
}
