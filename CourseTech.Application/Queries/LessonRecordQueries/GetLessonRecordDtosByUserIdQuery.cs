using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.LessonRecordQueries
{
    public record GetLessonRecordDtosByUserIdQuery(Guid UserId) : IRequest<LessonRecordDto[]>;
}
