using AutoMapper;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Mapping
{
    public class QuestionMapping : Profile
    {
        public QuestionMapping()
        {
            CreateMap<TestQuestion, TestQuestionDto>();

            // To Do здесь приходится делать Where фильтрацию и подтягивать все TestVariants, а как делать типо Outer apply в ef core?
            // To Do возможно добавить ProjectTo
            CreateMap<TestQuestion, TestQuestionCheckingDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CorrectVariant, opt => opt.MapFrom(src => src.TestVariants.FirstOrDefault(x => x.IsRight)));

            CreateMap<OpenQuestion, OpenQuestionDto>();

            CreateMap<OpenQuestion, OpenQuestionCheckingDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OpenQuestionsAnswers, opt => opt.MapFrom(src => src.AnswerVariants.Select(x => x.OpenQuestionCorrectAnswer)));

            CreateMap<PracticalQuestion, PracticalQuestionDto>();

            // To Do не уверен что так правильно
            CreateMap<PracticalQuestion, PracticalQuestionCheckingDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PracticalQuestionKeywords, opt => opt.MapFrom(src => src.QueryWords.SelectMany(x => x.Keyword.Word)));
        }
    }
}
