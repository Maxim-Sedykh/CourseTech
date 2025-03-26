﻿using CourseTech.Application.CQRS.Queries.Views;
using CourseTech.DAL.Views;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.ViewQueryHandlers;

public class GetQuestionTypeGradeHandler(IViewRepository<QuestionTypeGrade> questionTypeGradeRepository) : IRequestHandler<GetQuestionTypeGradeQuery, List<QuestionTypeGrade>>
{
    public async Task<List<QuestionTypeGrade>> Handle(GetQuestionTypeGradeQuery request, CancellationToken cancellationToken)
    {
        return await questionTypeGradeRepository.GetAllFromViewAsync();
    }
}
