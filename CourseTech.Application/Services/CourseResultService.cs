using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class CourseResultService(IBaseRepository<UserProfile> userProfileRepository, IBaseRepository<LessonRecord> lessonRecordRepository, IMapper mapper) : ICourseResultService
    {
        public async Task<BaseResult<CourseResultDto>> GetCourseResultAsync(Guid userId)
        {
            var profile = await userProfileRepository.GetAll()
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile is null)
            {
                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var analys = await CreateAnalys(profile);

            profile.Analys = analys.UserAnalys;
            profile.IsExamCompleted = true;

            userProfileRepository.Update(profile);
            await userProfileRepository.SaveChangesAsync();

            var courseResult = mapper.Map<CourseResultDto>(profile);

            return BaseResult<CourseResultDto>.Success(courseResult);
        }

        //To Do Check on null here
        public async Task<BaseResult<UserAnalysDto>> GetUserAnalys(Guid userId)
        {
            var userAnalysDto = await userProfileRepository.GetAll()
                .Where(x => x.UserId == userId)
                .Select(x => new UserAnalysDto() { Analys = x.Analys })
                .FirstOrDefaultAsync();

            if (userAnalysDto is null)
            {
                return BaseResult<UserAnalysDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);//UserAnalysNotFound
            }

            return BaseResult<UserAnalysDto>.Success(userAnalysDto);
        }

        private async Task<UserAnalysDto> CreateAnalys(UserProfile profile) // Создание анализа прохождения курса
        {
            var analys = new UserAnalysDto { UserAnalys = "Извините, данные вашего анализа были утеряны." };

            string firstPartOfAnalys = profile.CurrentGrade switch
            {
                > 60 and <= 75 => "Данный курс вы прошли удовлетворительно. ",
                > 75 and <= 90 => "Вы очень хорошо прошли курс! ",
                > 90 => "У вас отличный результат! Так держать!!! ",
                _ => "Вы прошли этот тест неудовлетворительно. Но не сдавайтесь! " +
                        "Вы всегда рано или поздно достигните успеха, если будете стараться!!! "
            };

            if (_lessonRecordRepository.GetAll().Where(x => x.UserId == profile.Id).Count() != supposedLessonRecordsCount)
            {
                return analys;
            }
            var usersLessonRecords = await _lessonRecordRepository.GetAll().Include(x => x.Lesson).Where(x => x.UserId == profile.Id).ToListAsync();
            var examLessonRecord = usersLessonRecords.Where(x => x.Lesson.Name == "Экзамен");
            var commonLessonRecords = usersLessonRecords.Except(examLessonRecord);

            var maxMarkLessonRecord = commonLessonRecords.OrderByDescending(x => x.Mark).First();
            var minMarkLessonRecord = commonLessonRecords.OrderByDescending(x => x.Mark).Last();

            string secondPartOfAnalys = $"Ваша средняя оценка по обычным занятиям {commonLessonRecords.Sum(x => x.Mark) / commonLessonRecords.Count()} " +
                 $"из 15 возможных. Вы набрали максимальное количество баллов по уроку {maxMarkLessonRecord.Lesson.Name} - {maxMarkLessonRecord.Mark} баллов." +
                 $" Минимальное по уроку {minMarkLessonRecord.Lesson.Name} - {minMarkLessonRecord.Mark} баллов," +
                 $" вы набрали {examLessonRecord.First().Mark} баллов по экзамену из 40б. " +
                 $"За одно тестовое задание можно было получить 1.5 балла, за открытое 3.5.";

            return new UserAnalysViewModel { UserAnalys = firstPartOfAnalys + secondPartOfAnalys };
        }
    }
}
