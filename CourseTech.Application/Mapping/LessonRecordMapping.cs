using AutoMapper;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Entities;

namespace CourseTech.Application.Mapping;

/// <summary>
/// Настройка маппинга для сущности "Запись о прохождения урока" в определённые DTO.
/// </summary>
public class LessonRecordMapping : Profile
{
    public LessonRecordMapping()
    {
        CreateMap<LessonRecord, LessonRecordDto>()
            .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson.Name))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString()));
    }
}
