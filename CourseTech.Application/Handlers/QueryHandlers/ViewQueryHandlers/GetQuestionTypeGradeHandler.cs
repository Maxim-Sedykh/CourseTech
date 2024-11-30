using AutoMapper;
using CourseTech.Application.Queries.Dtos.UserProfileDtoQuery;
using CourseTech.Application.Queries.Views;
using CourseTech.DAL.Views;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.QueryHandlers.ViewQueryHandlers
{
    public class GetQuestionTypeGradeHandler(IViewRepository<QuestionTypeGrade> questionTypeGradeRepository) : IRequestHandler<GetQuestionTypeGradeQuery, List<QuestionTypeGrade>>
    {
        public async Task<List<QuestionTypeGrade>> Handle(GetQuestionTypeGradeQuery request, CancellationToken cancellationToken)
        {
            return await questionTypeGradeRepository.GetAllFromViewAsync();
        }
    }
}
