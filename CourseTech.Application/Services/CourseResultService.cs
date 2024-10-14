using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Constants.LearningProcess;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseTech.Application.Services
{
    public class CourseResultService(IBaseRepository<UserProfile> userProfileRepository, IBaseRepository<LessonRecord> lessonRecordRepository,
        IBaseRepository<Lesson> lessonRepository, IMapper mapper, ICacheService cacheService) : ICourseResultService
    {
        public async Task<BaseResult<CourseResultDto>> GetCourseResultAsync(Guid userId)
        {
            var profile = await userProfileRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile is null)
            {
                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var usersLessonRecords = await lessonRecordRepository.GetAll()
                .Include(x => x.Lesson)
                .Where(x => x.UserId == profile.UserId)
                .Select(x => mapper.Map<LessonRecordDto>(x))
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            if (!usersLessonRecords.Any())
            {
                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.LessonRecordsNotFound, ErrorMessage.LessonRecordsNotFound);
            }

            var lessonsCount = await lessonRepository.GetAll().CountAsync();

            if (lessonsCount == 0)
            {
                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
            }

            var analys = CreateAnalys(profile.CurrentGrade, usersLessonRecords, lessonsCount);

            profile.Analys = analys.Analys;
            profile.IsExamCompleted = true;

            userProfileRepository.Update(profile);
            await userProfileRepository.SaveChangesAsync();

            await cacheService.RemoveAsync($"{CacheKeys.UserProfile}{profile.UserId}");

            var courseResult = mapper.Map<CourseResultDto>(profile);

            return BaseResult<CourseResultDto>.Success(courseResult);
        }

        public async Task<BaseResult<UserAnalysDto>> GetUserAnalys(Guid userId)
        {
            var userAnalys = await cacheService.GetOrAddToCache(
                $"{CacheKeys.UserAnalys}{userId}",
                async () =>
                {
                    return await userProfileRepository.GetAll()
                        .Where(x => x.UserId == userId)
                        .Select(x => mapper.Map<UserAnalysDto>(x))
                        .FirstOrDefaultAsync();
                });

            if (userAnalys == null)
            {
                return BaseResult<UserAnalysDto>.Failure((int)ErrorCodes.UserAnalysNotFound, ErrorMessage.UserAnalysNotFound);
            }

            return BaseResult<UserAnalysDto>.Success(userAnalys);
        }

        private UserAnalysDto CreateAnalys(float usersCurrentGrade, List<LessonRecordDto> userLessonRecords, int lessonsCount)
        {
            var analys = new UserAnalysDto { Analys = AnalysParts.UndefinedOverall };
            var examLessonRecord = userLessonRecords.LastOrDefault();

            if (userLessonRecords.Count != lessonsCount || examLessonRecord == null)
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

            var commonLessonRecords = userLessonRecords.Take(userLessonRecords.Count - 1).ToList();

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
