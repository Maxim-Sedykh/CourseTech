using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain;
using CourseTech.Domain.Dto.Keyword;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Parameters;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IQuestionAnswerChecker questionAnswerChecker) : IQuestionService
    {
        public async Task<BaseResult<LessonPracticeDto>> GetLessonQuestionsAsync(int lessonId)
        {
            var lesson = await unitOfWork.Lessons.GetAll().FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson is null)
            {
                return BaseResult<LessonPracticeDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            var questions = await unitOfWork.Questions.GetAll()
                .Where(q => q.LessonId == lessonId)
                .Select(q => mapper.MapQuestion(q))
                .ToListAsync();

            if (!questions.Any())
            {
                return BaseResult<LessonPracticeDto>.Failure((int)ErrorCodes.QuestionsNotFound, ErrorMessage.QuestionsNotFound);
            }

            return BaseResult<LessonPracticeDto>.Success(new LessonPracticeDto()
            {
                LessonId = lesson.Id,
                LessonType = lesson.LessonType,
                Questions = questions
            });
        }

        public async Task<BaseResult<PracticeCorrectAnswersDto>> PassLessonQuestionsAsync(PracticeUserAnswersDto dto, Guid userId)
        {
            var profile = await unitOfWork.UserProfiles.GetAll()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var lesson = await unitOfWork.Lessons.GetAll()
                .FirstOrDefaultAsync(x => x.Id == dto.LessonId);

            if (lesson == null)
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            var questions = await unitOfWork.Questions.GetAll()
                    .Where(q => q.LessonId == dto.LessonId)
                    .OrderBy(x => x.Number)
                    .ToListAsync();

            if (!questions.Any() || questions.Count != dto.UserAnswerDtos.Count
                || !questions.OfType<TestQuestion>().Any() || !questions.OfType<OpenQuestion>().Any())
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.LessonQuestionsNotFound,
                    ErrorMessage.LessonQuestionsNotFound);
            }

            var testQuestionsIds = questions.OfType<TestQuestion>().Select(x => x.Id);

            var correctTestQuestionsVariants = await unitOfWork.TestVariants.GetAll()
                        .Where(x => testQuestionsIds.Contains(x.QuestionId) && x.IsRight)
                        .Select(x => mapper.Map<TestVariantDto>(x))
                        .ToListAsync();

            if (!correctTestQuestionsVariants.Any())
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.TestQuestionsCorrectVariantsNotFound,
                    ErrorMessage.TestQuestionsCorrectVariantsNotFound);
            }

            var openQuestionsIds = questions.OfType<OpenQuestion>().Select(x => x.Id);

            var openQuestionsAnswerVariants = await unitOfWork.OpenQuestionAnswerVariants.GetAll()
                        .Where(x => openQuestionsIds.Contains(x.QuestionId))
                        .ToListAsync();

            if (!openQuestionsAnswerVariants.Any())
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.OpenQuestionsAnswerVariantsNotFound,
                    ErrorMessage.OpenQuestionsAnswerVariantsNotFound);
            }

            var practicalQuestions = await unitOfWork.PracticalQuestions.GetAll()
                .Include(x => x)
                .Where(x => x.LessonId == dto.LessonId)
                .ToListAsync();

            var practicalQuestionsIds = questions.OfType<PracticalQuestion>().Select(x => x.Id);

            var practicalQuestionsKeywords = unitOfWork.QueryWords.GetAll()
                .Where(x => practicalQuestionsIds.Contains(x.QuestionId))
                .OrderBy(x => x.Number)
                .Include(x => x.Keyword)
                .Select(x => new KeywordDto() { QuestionId = x.QuestionId, Keyword = x.Keyword.Word })
                .ToList();

            var practicalQuestionUserAnswerCheckDtos = questions.OfType<PracticalQuestion>().Select(x => new CheckPracticalQuestionDto()
            {
                Id = x.Id,
                CorrectQueryCode = x.RightQueryCode,
                PracticalQuestionKeywords = practicalQuestionsKeywords.Where(keyword => keyword.QuestionId == x.Id).Select(x => x.Keyword).ToList()
            }).ToList();

            var correctAnswers = CheckUserAnswers(new CheckUserAnswersRequest()
            {
                UserAnswers = dto.UserAnswerDtos,
                CorrectTestsVariants = correctTestQuestionsVariants,
                OpenQuestionsAnswerVariants = openQuestionsAnswerVariants,
                CheckPracticalQuestionDtos = practicalQuestionUserAnswerCheckDtos,
                PracticalQuestionsKeywords = practicalQuestionsKeywords
            }, out float userGrade);

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    profile.CurrentGrade += userGrade;
                    profile.LessonsCompleted++;

                    unitOfWork.UserProfiles.Update(profile);

                    LessonRecord lessonRecord = new LessonRecord()
                    {
                        LessonId = lesson.Id,
                        UserId = userId,
                        Mark = userGrade
                    };

                    await unitOfWork.LessonRecords.CreateAsync(lessonRecord);

                    await unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return BaseResult<PracticeCorrectAnswersDto>.Success(new PracticeCorrectAnswersDto()
            {
                LessonId = lesson.Id,
                QuestionCorrectAnswers = correctAnswers
            });
        }

        private List<ICorrectAnswerDto> CheckUserAnswers(CheckUserAnswersRequest requestParameters, out float userGrade)
        {
            var userAnswers = requestParameters.UserAnswers;
            var correctTestsVariants = requestParameters.CorrectTestsVariants;
            var openQuestionsAnswerVariants = requestParameters.OpenQuestionsAnswerVariants;
            var checkPracticalQuestionDtos = requestParameters.CheckPracticalQuestionDtos;
            var practicalQuestionsKeywords = requestParameters.PracticalQuestionsKeywords;

            var correctAnswers = new List<ICorrectAnswerDto>();

            userGrade = 0;

            foreach (var userAnswer in userAnswers)
            {
                var currentQuestionId = userAnswer.QuestionId;

                if (userAnswer is TestQuestionUserAnswerDto testUserAnswer)
                {
                    var correctTestVariant = requestParameters.CorrectTestsVariants
                        .FirstOrDefault(x => x.QuestionId == currentQuestionId);

                    correctAnswers.Add(questionAnswerChecker.CheckTestQuestionAnswer(testUserAnswer, correctTestVariant, ref userGrade));
                }
                else if (userAnswer is OpenQuestionUserAnswerDto openQuestionUserAnswerDto)
                {
                    var correctAnswersOnOpenQuestions = openQuestionsAnswerVariants
                        .Where(x => x.QuestionId == currentQuestionId)
                        .Select(x => x.OpenQuestionCorrectAnswer)
                        .ToList();

                    correctAnswers.Add(questionAnswerChecker.CheckOpenQuestionAnswer(openQuestionUserAnswerDto, correctAnswersOnOpenQuestions, ref userGrade));
                }
                else if (userAnswer is PracticalQuestionUserAnswerDto practicalQuestionUserAnswerDto)
                {
                    var rightQueryCode = checkPracticalQuestionDtos
                        .Where(x => x.Id == currentQuestionId)
                        .Select(x => x.CorrectQueryCode)
                        .FirstOrDefault();

                    var currentQuestionKeywords = practicalQuestionsKeywords
                        .Where(x => x.QuestionId == userAnswer.QuestionId)
                        .Select(x => x.Keyword)
                        .ToList();


                    correctAnswers.Add(questionAnswerChecker.CheckPracticalQuestionAnswer(practicalQuestionUserAnswerDto, rightQueryCode, currentQuestionKeywords, out float questionGrade));

                    userGrade += questionGrade;
                }
            }

            return correctAnswers;
        }
    }
}
