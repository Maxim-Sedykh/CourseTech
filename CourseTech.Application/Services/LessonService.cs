using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Helpers;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Parameters;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using QuickGraph;
using System.Net.WebSockets;

namespace CourseTech.Application.Services
{
    public class LessonService(IBaseRepository<Lesson> lessonRepository, IBaseRepository<UserProfile> userProfileRepository, IMapper mapper) : ILessonService
    {
        public async Task<BaseResult<LessonLectureDto>> GetLessonLectureAsync(int lessonId)
        {
            var lesson = await lessonRepository.GetAll()
                .Select(x => mapper.Map<LessonLectureDto>(x))
                .FirstOrDefaultAsync(x => x.Id == lessonId);

            if (lesson is null)
            {
                return BaseResult<LessonLectureDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            return BaseResult<LessonLectureDto>.Success(lesson);
        }

        public async Task<CollectionResult<LessonNameDto>> GetLessonNamesAsync()
        {
            var lessonNames = await lessonRepository.GetAll()
                .Select(x => mapper.Map<LessonNameDto>(x))
                .ToArrayAsync();

            if (lessonNames is null)
            {
                return CollectionResult<LessonNameDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
            }

            return CollectionResult<LessonNameDto>.Success(lessonNames);
        }

        public async Task<BaseResult<UserLessonsDto>> GetLessonsForUserAsync(Guid userId)
        {
            var profile = await userProfileRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
            {
                return BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var lessons = await lessonRepository.GetAll()
                .Select(x => mapper.Map<LessonDto>(x))
                .ToListAsync();

            if (lessons is null)
            {
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
            var currentLesson = await lessonRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (currentLesson == null)
            {
                return BaseResult<LessonLectureDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            currentLesson.LectureMarkup = dto.LessonMarkup.ToString();

            lessonRepository.Update(currentLesson);
            await lessonRepository.SaveChangesAsync();

            return BaseResult<LessonLectureDto>.Success(dto);
        }
    }
}