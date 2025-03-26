using CourseTech.Application.CQRS.Commands.LessonCommands;
using CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;
using CourseTech.Application.CQRS.Queries.Entities.LessonQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using MediatR;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services;

public class LessonService(
    ICacheService cacheService,
    IMediator mediator,
    ILogger logger,
    ILessonValidator lessonValidator) : ILessonService
{
    /// <inheritdoc/>
    public async Task<DataResult<LessonLectureDto>> GetLessonLectureAsync(int lessonId)
    {
        var lesson = await cacheService.GetOrAddToCache(
            $"{CacheKeys.LessonLecture}{lessonId}",
            async () => await mediator.Send(new GetLessonLectureDtoByIdQuery(lessonId)));

        if (lesson is null)
        {
            return DataResult<LessonLectureDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
        }

        return DataResult<LessonLectureDto>.Success(lesson);
    }

    /// <inheritdoc/>
    public async Task<CollectionResult<LessonNameDto>> GetLessonNamesAsync()
    {
        var lessonNames = await cacheService.GetOrAddToCache(
            CacheKeys.LessonNames,
            async () => await mediator.Send(new GetLessonNamesQuery()));

        if (!lessonNames.Any())
        {
            logger.Error(ErrorMessage.LessonsNotFound);

            return CollectionResult<LessonNameDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
        }

        return CollectionResult<LessonNameDto>.Success(lessonNames);
    }

    /// <inheritdoc/>
    public async Task<DataResult<UserLessonsDto>> GetLessonsForUserAsync(Guid userId)
    {
        var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

        var lessons = await mediator.Send(new GetLessonDtosQuery());

        var validateLessonsForUserResult = lessonValidator.ValidateLessonsForUser(profile, lessons);
        if (!validateLessonsForUserResult.IsSuccess)
        {
            return DataResult<UserLessonsDto>.Failure((int)validateLessonsForUserResult.Error.Code, validateLessonsForUserResult.Error.Message);
        }

        return DataResult<UserLessonsDto>.Success(new UserLessonsDto()
        {
            LessonNames = lessons,
            LessonsCompleted = profile.LessonsCompleted
        });
    }

    /// <inheritdoc/>
    public async Task<DataResult<LessonLectureDto>> UpdateLessonLectureAsync(LessonLectureDto dto)
    {
        var currentLesson = await mediator.Send(new GetLessonByIdQuery(dto.Id));
        if (currentLesson == null)
        {
            return DataResult<LessonLectureDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
        }

        if (HasChanges(currentLesson, dto))
        {
            await mediator.Send(new UpdateLessonCommand(dto, currentLesson));

            await RemoveOldCacheAsync(currentLesson, dto);
        }

        return DataResult<LessonLectureDto>.Success(dto);
    }

    /// <summary>
    /// Сравнивает свойства модели LessonLectureDto и сущности Lesson
    /// </summary>
    /// <param name="lesson"></param>
    /// <param name="dto"></param>
    /// <returns>Если свойства разные - возвращает false</returns>
    private bool HasChanges(Lesson lesson, LessonLectureDto dto)
    {
        return lesson.Name != dto.Name ||
               lesson.LessonType != dto.LessonType ||
               lesson.LectureMarkup != dto.LectureMarkup;
    }

    /// <summary>
    /// Очистка устаревшего кэша после обновления урока
    /// </summary>
    /// <param name="lesson"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    private async Task RemoveOldCacheAsync(Lesson lesson, LessonLectureDto dto)
    {
        if (lesson.Name != dto.Name)
        {
            await cacheService.RemoveAsync(CacheKeys.LessonNames);
        }

        await cacheService.RemoveAsync($"{CacheKeys.LessonLecture}{lesson.Id}");
    }
}