﻿using CourseTech.Application.CQRS.Commands.LessonRecordCommands;
using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;
using CourseTech.Application.CQRS.Queries.Entities.LessonQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Application.CQRS.Queries.Views;
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

namespace CourseTech.Application.Services;

public class QuestionService(
    ICacheService cacheService,
    IMediator mediator,
    IUnitOfWork unitOfWork,
    IQuestionAnswerChecker questionAnswerChecker,
    IQuestionValidator questionValidator,
    ILogger logger) : IQuestionService
{
    /// <inheritdoc/>
    public async Task<DataResult<LessonPracticeDto>> GetLessonQuestionsAsync(int lessonId, bool isDemoMode)
    {
        var lesson = await mediator.Send(new GetLessonByIdQuery(lessonId));
        var questions = await mediator.Send(new GetLessonQuestionDtosQuery(lessonId, isDemoMode));

        var lessonQuestionsValidationResult = questionValidator.ValidateLessonQuestions(lesson, questions);
        if (!lessonQuestionsValidationResult.IsSuccess)
        {
            return DataResult<LessonPracticeDto>.Failure((int)lessonQuestionsValidationResult.Error.Code,
                lessonQuestionsValidationResult.Error.Message);
        }

        return DataResult<LessonPracticeDto>.Success(new LessonPracticeDto()
        {
            LessonId = lesson.Id,
            LessonType = lesson.LessonType,
            IsDemoMode = isDemoMode,
            Questions = questions
        });
    }

    /// <inheritdoc/>
    public async Task<DataResult<PracticeCorrectAnswersDto>> PassLessonQuestionsAsync(PracticeUserAnswersDto dto, Guid userId)
    {
        var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

        var lesson = await mediator.Send(new GetLessonByIdQuery(dto.LessonId));

        var validationResult = questionValidator.ValidateUserLessonOnNull(profile, lesson);
        if (!validationResult.IsSuccess)
        {
            return DataResult<PracticeCorrectAnswersDto>.Failure((int)validationResult.Error.Code, validationResult.Error.Message);
        }

        var questions = await mediator.Send(new GetLessonCheckQuestionDtosQuery(lesson.Id, dto.IsDemoMode));

        var questionValidationResult = questionValidator.ValidateQuestions(questions, dto.UserAnswerDtos.Count, lesson.LessonType);
        if (!questionValidationResult.IsSuccess)
        {
            return DataResult<PracticeCorrectAnswersDto>.Failure((int)questionValidationResult.Error.Code, questionValidationResult.Error.Message);
        }

        var userGrade = new UserGradeDto()
        {
            Grade = profile.CurrentGrade
        };

        var questionTypeGrades = await mediator.Send(new GetQuestionTypeGradeQuery());

        var correctAnswers = await questionAnswerChecker.CheckUserAnswers(questions, dto.UserAnswerDtos, userGrade, questionTypeGrades);

        if (correctAnswers.Count == 0 && lesson.Name != "Экзамен")
        {
            return DataResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.AnswerCheckError, ErrorMessage.AnswerCheckError);
        }

        if (profile.LessonsCompleted < lesson.Number)
        {
            await UpdateProfileAndCreateLessonRecord(profile, lesson.Id, userGrade.Grade, dto.IsDemoMode);
        }

        return DataResult<PracticeCorrectAnswersDto>.Success(new PracticeCorrectAnswersDto()
        {
            LessonId = lesson.Id,
            QuestionCorrectAnswers = correctAnswers
        });
    }

    /// <summary>
    /// Обновление профиля и создание записи о прохождении урока в транзакции
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="lessonId"></param>
    /// <param name="userGrade"></param>
    /// <returns></returns>
    private async Task UpdateProfileAndCreateLessonRecord(UserProfile profile, int lessonId, float userGrade, bool isDemoMode)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var userId = profile.UserId;

            if (isDemoMode)
            {
                var lr = new LessonRecord()
                {
                    UserId = userId,
                    LessonId = lessonId,
                    Mark = userGrade,
                    IsDemo = true
                };
            }
            else
            {
                await mediator.Send(new UpdateProfileCompletingLessonCommand(profile, userGrade));

                userGrade += userGrade * 0.1f;

                await mediator.Send(new CreateLessonRecordCommand(userId, lessonId, userGrade));
            }

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
