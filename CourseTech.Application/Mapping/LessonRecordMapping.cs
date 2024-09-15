using AutoMapper;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Mapping
{
    public class LessonRecordMapping : Profile
    {
        public LessonRecordMapping()
        {
            CreateMap<LessonRecord, LessonRecordDto>()
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToLongDateString()));
        }
    }
}
