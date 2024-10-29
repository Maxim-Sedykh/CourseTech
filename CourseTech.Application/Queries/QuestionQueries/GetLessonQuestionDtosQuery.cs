using CourseTech.Domain.Interfaces.Dtos.Question;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.QuestionQueries
{
    public record GetLessonQuestionDtosQuery(int LessonId) : IRequest<List<IQuestionDto>>;
}
