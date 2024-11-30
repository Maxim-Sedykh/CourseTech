using CourseTech.DAL.Views;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Views
{
    /// <summary>
    /// Получение представления <see cref="QuestionTypeGrade"/>
    /// </summary>
    /// <param name="UserId"></param>
    public record GetQuestionTypeGradeQuery() : IRequest<List<QuestionTypeGrade>>;
}
