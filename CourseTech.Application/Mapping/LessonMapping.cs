using AutoMapper;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Mapping
{
    public class LessonMapping : Profile
    {
        public LessonMapping()
        {
            CreateMap<Lesson, LessonLectureDto>()
                .ForMember(dest => dest.LessonMarkup, opt => opt.MapFrom(src => new HtmlString(src.LectureMarkup)));

            CreateMap<Lesson, LessonNameDto>();

            CreateMap<Lesson, LessonDto>();
        }
    }
}
