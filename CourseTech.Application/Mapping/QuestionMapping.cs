using AutoMapper;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseTech.Application.Mapping
{
    public class QuestionMapping : Profile
    {
        public QuestionMapping()
        {
            CreateMap<TestQuestion, TestQuestionDto>()
                .ForMember(dest => dest.TestVariants, opt => opt.MapFrom(src => src.TestVariants.Select(x => new TestVariantDto()
                {
                    QuestionId = x.TestQuestionId,
                    Content = x.Content,
                    VariantNumber = x.VariantNumber,
                }))); //To Do как здесь это делать правильно?

            // To Do здесь приходится делать Where фильтрацию и подтягивать все TestVariants, а как делать типо Outer apply в ef core?
            // To Do возможно добавить ProjectTo
            CreateMap<TestQuestion, TestQuestionCheckingDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CorrectVariant, opt => 
                opt.MapFrom(src => src.TestVariants
                .Where(x => x.IsCorrect)
                .Select(x => new TestVariantDto() 
                { 
                    QuestionId = x.TestQuestionId, 
                    Content = x.Content, 
                    VariantNumber = x.VariantNumber
                })
                .FirstOrDefault()));

            CreateMap<OpenQuestion, OpenQuestionDto>();

            CreateMap<OpenQuestion, OpenQuestionCheckingDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OpenQuestionsAnswers, opt => opt.MapFrom(src => src.AnswerVariants.Select(x => x.AnswerText)));

            CreateMap<PracticalQuestion, PracticalQuestionDto>();

            // To Do не уверен что так правильно
            CreateMap<PracticalQuestion, PracticalQuestionCheckingDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PracticalQuestionKeywords, opt => opt.MapFrom(src => src.QueryWords.Select(x => x.Keyword.Word).ToList()));
        }
    }
}
