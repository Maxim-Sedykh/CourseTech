﻿using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.QuestionDtoQueryHandlers;

public class GetLessonQuestionDtosHandler(IBaseRepository<BaseQuestion> questionRepository, IMapper mapper) : IRequestHandler<GetLessonQuestionDtosQuery, List<IQuestionDto>>
{
    public async Task<List<IQuestionDto>> Handle(GetLessonQuestionDtosQuery request, CancellationToken cancellationToken)
    {
        var res = await questionRepository.GetAll()
            .Include(q => (q as TestQuestion).TestVariants)
            .Where(q => q.LessonId == request.LessonId)
            .Select(q => mapper.MapQuestion(q))
            .ToListAsync(cancellationToken);

        if (request.IsDemoMode)
        {
            res = [.. res.Except(res.Where(x => x is PracticalQuestionDto))];
        }

        return res;
    }
}
