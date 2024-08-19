using Microsoft.EntityFrameworkCore;
using ServiceStack;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.FinalResult;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ServiceStack.Svg;

namespace CourseTech.Application.Services
{
    public class FinalResultService : IFinalResultService
    {
        private const byte supposedLessonRecordsCount = 5;
        private readonly IBaseRepository<UserProfile> _userProfileRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;

        public FinalResultService(IBaseRepository<UserProfile> userProfileRepository, IBaseRepository<LessonRecord> lessonRecordRepository)
        {
            _userProfileRepository = userProfileRepository;
            _lessonRecordRepository = lessonRecordRepository;
        }

        public async Task<IBaseResponse<ResultViewModel>> GetResultModel(string userLogin)
        {
            try
            {
                var profile = await _userProfileRepository.GetAll()
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.Login == userLogin);
                if (profile is null)
                {
                    return new BaseResponse<ResultViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                var analys = await CreateAnalys(profile);

                profile.Analys = analys.UserAnalys;
                profile.IsExamCompleted = true;

                await _userProfileRepository.Update(profile);

                var response = new ResultViewModel()
                {
                    Id = profile.Id,
                    Login = profile.User.Login,
                    Name = profile.Name,
                    Surname = profile.Surname,
                    CurrentGrade = profile.CurrentGrade,
                    UserAnalys = profile.Analys
                };

                if (response is null)
                {
                    return new BaseResponse<ResultViewModel>()
                    {
                        Description = "Уроки не найдены",
                        StatusCode = StatusCode.LessonNotFound
                    };
                }

                return new BaseResponse<ResultViewModel>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ResultViewModel>()
                {
                    Description = $"Внутренняя ошибка : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }


        }

        private async Task<UserAnalysViewModel> CreateAnalys(UserProfile profile) // Создание анализа прохождения курса
        {
            var analys = new UserAnalysViewModel { UserAnalys = "Извините, данные вашего анализа были утеряны." };

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

        public async Task<IBaseResponse<UserAnalysViewModel>> GetUserAnalys(long id)
        {
            try
            {
                var profile = await _userProfileRepository.GetAll()
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.Id == id);

                if (profile is null)
                {
                    return new BaseResponse<UserAnalysViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }
                if (profile.Analys is null)
                {
                    return new BaseResponse<UserAnalysViewModel>()
                    {
                        Description = "Извините, мы не сумели обнаружить ваш анализ",
                        StatusCode = StatusCode.UserAnalysNotFound
                    };
                }
                return new BaseResponse<UserAnalysViewModel>()
                {
                    Data = new UserAnalysViewModel { UserAnalys = profile.Analys },
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserAnalysViewModel>()
                {
                    Description = $"Внутренняя ошибка : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
