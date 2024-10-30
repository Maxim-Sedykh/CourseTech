using CourseTech.Application.Commands.LessonRecordCommands;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Application.Queries.QuestionQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using MediatR;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services
{
    public class QuestionService(IUnitOfWork unitOfWork, IQuestionAnswerChecker questionAnswerChecker,
        IQuestionValidator questionValidator, ICacheService cacheService, IMediator mediator, ILogger logger) : IQuestionService
    {
        public async Task<BaseResult<LessonPracticeDto>> GetLessonQuestionsAsync(int lessonId)
        {
            var lesson = await mediator.Send(new GetLessonByIdQuery(lessonId));
            if (lesson is null)
            {
                return BaseResult<LessonPracticeDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            var questions = await mediator.Send(new GetLessonQuestionDtosQuery(lesson.Id));

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
            var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

            var lesson = await mediator.Send(new GetLessonByIdQuery(dto.LessonId));

            var validationResult = questionValidator.ValidateUserLessonOnNull(profile, lesson);
            if (!validationResult.IsSuccess)
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)validationResult.Error.Code, validationResult.Error.Message);
            }

            var questions = await mediator.Send(new GetLessonCheckQuestionDtosQuery(lesson.Id));

            var questionValidationResult = questionValidator.ValidateQuestions(questions, dto.UserAnswerDtos.Count, lesson.LessonType);
            if (!questionValidationResult.IsSuccess)
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)questionValidationResult.Error.Code, questionValidationResult.Error.Message);
            }

            var userGrade = new UserGradeDto()
            {
                Grade = profile.CurrentGrade
            };

            var correctAnswers = await questionAnswerChecker.CheckUserAnswers(questions, dto.UserAnswerDtos, userGrade);

            if (correctAnswers.Any())
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.AnswerCheckError, ErrorMessage.AnswerCheckError);
            }

            await UpdateProfileAndCreateLessonRecord(profile, lesson.Id, userGrade.Grade);

            return BaseResult<PracticeCorrectAnswersDto>.Success(new PracticeCorrectAnswersDto()
            {
                LessonId = lesson.Id,
                QuestionCorrectAnswers = correctAnswers
            });
        }

        private async Task UpdateProfileAndCreateLessonRecord(UserProfile profile, int lessonId, float userGrade)
        {
            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var userId = profile.UserId;

                    await mediator.Send(new UpdateProfileCompletingLessonCommand(profile, userGrade));
                    await mediator.Send(new CreateLessonRecordCommand(userId, lessonId, userGrade));

                    await unitOfWork.SaveChangesAsync();

                    await cacheService.RemoveAsync($"{CacheKeys.UserProfile}{userId}");
                    await cacheService.RemoveAsync($"{CacheKeys.UserLessonRecords}{userId}");

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, ex.Message);

                    await transaction.RollbackAsync();
                }
            }
        }
    }
}
