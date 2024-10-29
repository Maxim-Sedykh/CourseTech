using AutoMapper;
using CourseTech.Application.Queries.QuestionQueries;
using CourseTech.Application.Queries.Reviews;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CourseTech.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.QuestionHandlers
{
    public class GetLessonQuestionDtosHandler(IBaseRepository<Question> questionRepository, IMapper mapper) : IRequestHandler<GetLessonQuestionDtosQuery, List<IQuestionDto>>
    {
        public async Task<List<IQuestionDto>> Handle(GetLessonQuestionDtosQuery request, CancellationToken cancellationToken)
        {
            return await questionRepository.GetAll()
                .Include(q => (q as TestQuestion).TestVariants)
                .Where(q => q.LessonId == request.LessonId)
                .Select(q => mapper.MapQuestion(q))
                .ToListAsync(cancellationToken);
        }
    }
}
