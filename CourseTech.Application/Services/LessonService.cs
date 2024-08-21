using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Services
{
    public class LessonService : ILessonService
    {
        public Task<BaseResult<LessonLectureDto>> GetLessonLectureAsync(int lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<CollectionResult<LessonNameDto>> GetLessonNamesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<LessonPassDto>> GetLessonQuestionsAsync(int lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UserLessonsDto>> GetLessonsForUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<LessonPassDto>> PassLessonAsync(LessonPassDto dto, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<LessonLectureDto>> UpdateLessonLectureAsync(LessonLectureDto dto)
        {
            throw new NotImplementedException();
        }
    }
}