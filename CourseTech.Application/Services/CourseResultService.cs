using AutoMapper;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Application.Queries.LessonRecordQueries;
using CourseTech.Application.Queries.UserProfileQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Constants.LearningProcess;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services
{
    public class CourseResultService(
        IMapper mapper,
        ICacheService cacheService,
        IMediator mediator,
        ILogger logger) : ICourseResultService
    {

        public async Task<BaseResult<CourseResultDto>> GetCourseResultAsync(Guid userId)
        {
            var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

            if (profile is null)
            {
                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var userLessonRecords = await mediator.Send(new GetLessonRecordDtosByUserIdQuery(userId));

            var lessonsCount = await mediator.Send(new GetLessonsCountQuery());

            if (lessonsCount == 0)
            {
                logger.Error(ErrorMessage.LessonsNotFound);

                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
            }

            var analysDto = CreateAnalys(profile.CurrentGrade, userLessonRecords, lessonsCount);

            await mediator.Send(new UpdateCompletedCourseUserProfileCommand(profile, analysDto.Analys));

            await cacheService.RemoveAsync($"{CacheKeys.UserProfile}{profile.UserId}");

            return BaseResult<CourseResultDto>.Success(mapper.Map<CourseResultDto>(profile));
        }

        public async Task<BaseResult<UserAnalysDto>> GetUserAnalys(Guid userId)
        {
            var userAnalys = await cacheService.GetOrAddToCache(
                $"{CacheKeys.UserAnalys}{userId}",
                async () => await mediator.Send(new GetAnalysByUserIdQuery(userId)));

            if (userAnalys == null)
            {
                return BaseResult<UserAnalysDto>.Failure((int)ErrorCodes.UserAnalysNotFound, ErrorMessage.UserAnalysNotFound);
            }

            return BaseResult<UserAnalysDto>.Success(userAnalys);
        }

        private UserAnalysDto CreateAnalys(float usersCurrentGrade, LessonRecordDto[] userLessonRecords, int lessonsCount)
        {
            var analys = new UserAnalysDto { Analys = AnalysParts.UndefinedOverall };
            var examLessonRecord = userLessonRecords.LastOrDefault();

            if (userLessonRecords.Length != lessonsCount || examLessonRecord == null)
            {
                return analys;
            }

            string firstPartOfAnalys = usersCurrentGrade switch
            {
                > 90 => AnalysParts.ExcellentOverall,
                > 75 => AnalysParts.GoodOverall,
                > 60 => AnalysParts.SatisfactoryOverall,
                _ => AnalysParts.UnsatisfactoryOverall
            };

            var commonLessonRecords = userLessonRecords.Take(userLessonRecords.Length - 1).ToList();

            if (commonLessonRecords.Count > 0)
            {
                var averageMark = commonLessonRecords.Average(x => x.Mark);
                var maxMarkCommonLessonRecord = commonLessonRecords.OrderByDescending(x => x.Mark).First();

                string secondPartOfAnalys = string.Format(AnalysParts.LessonsAnalys,
                    averageMark,
                    maxMarkCommonLessonRecord.LessonName,
                    maxMarkCommonLessonRecord.Mark,
                    examLessonRecord.Mark);

                analys.Analys = $"{firstPartOfAnalys} - {secondPartOfAnalys}";
            }

            return analys;
        }
    }
}
