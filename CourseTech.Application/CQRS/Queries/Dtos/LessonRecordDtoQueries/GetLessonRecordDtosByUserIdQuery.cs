using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonRecordDtoQueries
{
    /// <summary>
    /// Получение записей о прохождении уроков в виде LessonRecordDto, для определённого пользователя по его идентификатору.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetLessonRecordDtosByUserIdQuery(Guid UserId) : IRequest<LessonRecordDto[]>;
}
