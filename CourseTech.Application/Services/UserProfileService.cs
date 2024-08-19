using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Domain.ViewModels.UserProfile;
using SQLServerCourse.Service.Interfaces;
using System.Security.Claims;

namespace CourseTech.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserProfile> _userProfileRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;
        private readonly IBaseRepository<Lesson> _lessonRepository;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(IBaseRepository<User> userRepository, IBaseRepository<UserProfile> userProfileRepository,
            IBaseRepository<LessonRecord> lessonRecordRepository, IBaseRepository<Lesson> lessonRepository, ILogger<UserProfileService> logger)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _lessonRecordRepository = lessonRecordRepository;
            _lessonRepository = lessonRepository;
            _logger = logger;
        }

        public async Task<IBaseResponse<UserProfileViewModel>> GetUserProfile(string userLogin)
        {
            try
            {
                var result = await _userProfileRepository.GetAll()
                    .Include(x => x.User)
                    .Select(x => new UserProfileViewModel()
                    {
                        Id = x.Id,
                        Login = x.User.Login,
                        Name = x.Name,
                        Surname = x.Surname,
                        Age = x.Age,
                        CurrentGrade = x.CurrentGrade,
                        IsExamCompleted = x.IsExamCompleted,
                        LessonsCompleted = x.LessonsCompleted,
                        IsEditAble = x.IsEditAble
                    })
                    .FirstOrDefaultAsync(x => x.Login == userLogin);

                if (result is null)
                {
                    return new BaseResponse<UserProfileViewModel>()
                    {
                        Description = "Ваш профиль не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                return new BaseResponse<UserProfileViewModel>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserProfileService.GetUserProfile] error: {ex.Message}");
                return new BaseResponse<UserProfileViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<UserProfile>> UpdateInfo(UserProfileViewModel model)
        {
            try
            {
                var profile = await _userProfileRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                if (profile == null)
                {
                    return new BaseResponse<UserProfile>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                profile.Name = model.Name;
                profile.Surname = model.Surname;
                profile.Age = model.Age;

                await _userProfileRepository.Update(profile);

                return new BaseResponse<UserProfile>()
                {
                    Data = profile,
                    Description = "Данные обновлены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[UserProfileService.UpdateInfo] error: {ex.Message}");
                return new BaseResponse<UserProfile>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<LessonRecordViewModel>>> GetLessonRecords(long id)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return new BaseResponse<List<LessonRecordViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var response = await _lessonRecordRepository.GetAll()
                    .Include(lessonrec => lessonrec.Lesson)
                    .Where(lessonrec => lessonrec.UserId == user.Id)
                    .Select(lessonRecordViewModel => new LessonRecordViewModel
                    {
                        LessonName = lessonRecordViewModel.Lesson.Name,
                        LessonMark = lessonRecordViewModel.Mark,
                        DateOfReceiving = lessonRecordViewModel.DateOfReceiving,
                    }).ToListAsync();

                return new BaseResponse<List<LessonRecordViewModel>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<LessonRecordViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public async Task<IBaseResponse<LessonListViewModel>> GetLessonList(string userLogin)
        {
            try
            {
                var profile = await _userProfileRepository.GetAll()
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.Login == userLogin);
                if (profile == null)
                {
                    return new BaseResponse<LessonListViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                var lessons = _lessonRepository.GetAll().Select(x => x.Name).ToList();
                if (!lessons.Any())
                {
                    return new BaseResponse<LessonListViewModel>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<LessonListViewModel>()
                {
                    Data = new LessonListViewModel { LessonsCompleted = profile.LessonsCompleted, LessonNames = lessons },
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LessonListViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
