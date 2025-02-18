using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Dtos.Question;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries
{
    /// <summary>
    /// Получение списка CheckQuestionDto для вопросов по определённому уроку.
    /// </summary>
    /// <param name="LessonId"></param>
    public record GetLessonCheckQuestionDtosQuery(int LessonId) : IRequest<List<ICheckQuestionDto>>;
}
