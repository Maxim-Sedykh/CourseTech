using CourseTech.Application.Commands.LessonCommands;
using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services
{
    public class LessonService(ICacheService cacheService, IMediator mediator, ILogger logger) : ILessonService
    {
        public async Task<BaseResult<LessonLectureDto>> GetLessonLectureAsync(int lessonId)
        {
            var lesson = await cacheService.GetOrAddToCache(
                $"{CacheKeys.LessonLecture}{lessonId}",
                async () => await mediator.Send(new GetLessonLectureDtoByIdQuery(lessonId)));

            if (lesson is null)
            {
                return BaseResult<LessonLectureDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            return BaseResult<LessonLectureDto>.Success(lesson);
        }

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

        public async Task<BaseResult<UserLessonsDto>> GetLessonsForUserAsync(Guid userId)
        {
            var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

            if (profile == null)
            {
                return BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var lessons = await mediator.Send(new GetLessonDtosQuery());

            if (!lessons.Any())
            {
                logger.Error(ErrorMessage.LessonsNotFound);

                return BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
            }

            return BaseResult<UserLessonsDto>.Success(new UserLessonsDto()
            {
                LessonNames = lessons,
                LessonsCompleted = profile.LessonsCompleted
            });
        }

        public async Task<BaseResult<LessonLectureDto>> UpdateLessonLectureAsync(LessonLectureDto dto)
        {
            var currentLesson = await mediator.Send(new GetLessonByIdQuery(dto.Id));
            if (currentLesson == null)
            {
                return BaseResult<LessonLectureDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            if (HasChanges(currentLesson, dto))
            {
                await mediator.Send(new UpdateLessonCommand(dto, currentLesson));

                await RemoveOldCacheAsync(currentLesson, dto);
            }

            return BaseResult<LessonLectureDto>.Success(dto);
        }

        private bool HasChanges(Lesson lesson, LessonLectureDto dto)
        {
            return lesson.Name != dto.Name ||
                   lesson.LessonType != dto.LessonType ||
                   lesson.LectureMarkup != dto.LessonMarkup.ToString();
        }

        private async Task RemoveOldCacheAsync(Lesson lesson, LessonLectureDto dto)
        {
            if (lesson.Name != dto.Name)
            {
                await cacheService.RemoveAsync(CacheKeys.LessonNames);
            }

            await cacheService.RemoveAsync($"{CacheKeys.LessonLecture}{lesson.Id}");
        }
    }
}