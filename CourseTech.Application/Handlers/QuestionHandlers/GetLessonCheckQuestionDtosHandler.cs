using AutoMapper;
using CourseTech.Application.Queries.QuestionQueries;
using CourseTech.DAL.Repositories;
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
    public class GetLessonCheckQuestionDtosHandler(IBaseRepository<Question> questionRepository, IMapper mapper) : IRequestHandler<GetLessonCheckQuestionDtosQuery, List<ICheckQuestionDto>>
    {
        public async Task<List<ICheckQuestionDto>> Handle(GetLessonCheckQuestionDtosQuery request, CancellationToken cancellationToken)
        {
            return await questionRepository.GetAll()
                .Where(q => q.LessonId == request.LessonId)
                .Include(q => (q as TestQuestion).TestVariants)
                .Include(q => (q as OpenQuestion).AnswerVariants)
                .Include(q => (q as PracticalQuestion).QueryWords)
                .ThenInclude(qw => qw.Keyword)
                .Select(x => mapper.MapQuestionCheckings(x))
                .ToListAsync(cancellationToken);
        }
    }
}
