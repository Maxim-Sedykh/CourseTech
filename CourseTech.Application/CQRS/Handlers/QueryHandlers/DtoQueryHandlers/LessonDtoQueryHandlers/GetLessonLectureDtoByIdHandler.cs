using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.LessonDtoQueryHandlers;

public class GetLessonLectureDtoByIdHandler(IBaseRepository<Lesson> lessonRepository, IMapper mapper) : IRequestHandler<GetLessonLectureDtoByIdQuery, LessonLectureDto>
{
    public async Task<LessonLectureDto> Handle(GetLessonLectureDtoByIdQuery request, CancellationToken cancellationToken)
    {
        return await lessonRepository.GetAll()
                    .Where(x => x.Id == request.LessonId)
                    .AsProjected<Lesson, LessonLectureDto>(mapper)
                    .FirstOrDefaultAsync(cancellationToken);
    }
}
