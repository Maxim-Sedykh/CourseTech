using AutoMapper;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;

namespace CourseTech.Application.Mapping;

/// <summary>
/// Настройка маппинга для сущности "Урок" в определённые DTO.
/// </summary>
public class LessonMapping : Profile
{
    public LessonMapping()
    {
        CreateMap<Lesson, LessonLectureDto>().ReverseMap();

        CreateMap<Lesson, LessonNameDto>();

        CreateMap<Lesson, LessonDto>();
    }
}
