using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain;
using CourseTech.Domain.Dto.Keyword;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Dto.OpenQuestionAnswer;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Parameters;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IQuestionAnswerChecker questionAnswerChecker, IQuestionValidator questionValidator) : IQuestionService
    {
        public async Task<BaseResult<LessonPracticeDto>> GetLessonQuestionsAsync(int lessonId)
        {
            var lesson = await unitOfWork.Lessons.GetAll().FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson is null)
            {
                return BaseResult<LessonPracticeDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            var questions = await unitOfWork.Questions.GetAll()
                .Include(q => (q as TestQuestion).TestVariants)
                .Where(q => q.LessonId == lessonId)
                .Select(q => mapper.MapQuestion(q))
                .ToListAsync();

            if (!questions.Any())
            {
                return BaseResult<LessonPracticeDto>.Failure((int)ErrorCodes.QuestionsNotFound, ErrorMessage.QuestionsNotFound);
            }

            return BaseResult<LessonPracticeDto>.Success(new LessonPracticeDto()
            {
                LessonId = lesson.Id,
                LessonType = lesson.LessonType,
                Questions = questions
            });
        }

        public async Task<BaseResult<PracticeCorrectAnswersDto>> PassLessonQuestionsAsync(PracticeUserAnswersDto dto, Guid userId)
        {
            var profile = await unitOfWork.UserProfiles.GetAll()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var lesson = await unitOfWork.Lessons.GetAll()
                .FirstOrDefaultAsync(x => x.Id == dto.LessonId);

            var validationResult = questionValidator.ValidateUserLessonOnNull(profile, lesson);
            if (!validationResult.IsSuccess)
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)validationResult.Error.Code, validationResult.Error.Message);
            }

            // To Do оптимизировать
            var questions = await unitOfWork.Questions.GetAll()
                .Where(q => q.LessonId == dto.LessonId)
                .Include(q => (q as TestQuestion).TestVariants)
                .Include(q => (q as OpenQuestion).AnswerVariants)
                .Include(q => (q as PracticalQuestion).QueryWords)
                .ThenInclude(qw => qw.Keyword)
                .Select(x => mapper.MapQuestionCheckings(x))
                .ToListAsync();

            var questionValidationResult = questionValidator.ValidateQuestions(questions, dto.UserAnswerDtos.Count, lesson.LessonType);
            if (!questionValidationResult.IsSuccess)
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)questionValidationResult.Error.Code, questionValidationResult.Error.Message);
            }

            var correctAnswers = questionAnswerChecker.CheckUserAnswers(questions, dto.UserAnswerDtos, out float userGrade);

            await UpdateProfileAndCreateLessonRecord(profile, lesson, userId, userGrade);

            return BaseResult<PracticeCorrectAnswersDto>.Success(new PracticeCorrectAnswersDto()
            {
                LessonId = lesson.Id,
                QuestionCorrectAnswers = correctAnswers
            });
        }

        private async Task UpdateProfileAndCreateLessonRecord(UserProfile profile, Lesson lesson, Guid userId, float userGrade)
        {
            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    profile.CurrentGrade += userGrade;
                    profile.LessonsCompleted++;

                    unitOfWork.UserProfiles.Update(profile);

                    await unitOfWork.LessonRecords.CreateAsync(new LessonRecord()
                    {
                        LessonId = lesson.Id,
                        UserId = userId,
                        Mark = userGrade
                    });

                    await unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
        }
    }
}
