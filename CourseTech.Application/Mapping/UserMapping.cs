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

            //To Do может это можно как-то оптимизировать
            CreateMap<User, UpdateUserDto>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.UserProfile.Name))
                .ForPath(dest => dest.Surname, opt => opt.MapFrom(src => src.UserProfile.Surname))
                .ForPath(dest => dest.IsEditAble, opt => opt.MapFrom(src => src.UserProfile.IsEditAble))
                .ForMember(d => d.Id, (options) => options.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForPath(dest => dest.UserProfile.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.UserProfile.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForPath(dest => dest.UserProfile.IsEditAble, opt => opt.MapFrom(src => src.IsEditAble));
        }
    }
}
