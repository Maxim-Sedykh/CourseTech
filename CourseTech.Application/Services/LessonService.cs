using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
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
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using QuickGraph;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Azure.Core.HttpHeader;

namespace CourseTech.Application.Services
{
    public class LessonService(IUnitOfWork unitOfWork, IMapper mapper) : ILessonService
    {
        public async Task<BaseResult<LessonLectureDto>> GetLessonLectureAsync(int lessonId)
        {
            var lesson = await unitOfWork.Lessons.GetAll()
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
            var lessonNames = await unitOfWork.Lessons.GetAll()
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
            var profile = await unitOfWork.UserProfiles.GetAll().FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
            {
                return BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var lessons = await unitOfWork.Lessons.GetAll()
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
            var currentLesson = await unitOfWork.Lessons.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (currentLesson == null)
            {
                return BaseResult<LessonLectureDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            currentLesson.LectureMarkup = dto.LessonMarkup.ToString();

            unitOfWork.Lessons.Update(currentLesson);
            await unitOfWork.SaveChangesAsync();

            return BaseResult<LessonLectureDto>.Success(dto);
        }

        public async Task<BaseResult<LessonPracticeDto>> GetLessonQuestionsAsync(int lessonId)
        {
            var lesson = await unitOfWork.Lessons.GetAll().FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson is null)
            {
                return BaseResult<LessonPracticeDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            // To Do 3 разных репозитория юзаем, не уверен что тут так надо делать
            var lessonQuestions = await unitOfWork.OpenQuestions.GetAll()  // To Do посмотреть какой запрос здесь формируется, если что -- переписать запрос
                .Where(q => q.LessonId == lessonId)
                .Select(x => mapper.Map<OpenQuestionDto>(x)).Cast<IQuestionDto>()
                .Union(unitOfWork.PracticalQuestions.GetAll()
                .Where(q => q.LessonId == lessonId)
                .Select(x => mapper.Map<PracticalQuestionDto>(x)))
                .Union(unitOfWork.TestQuestions.GetAll()
                .Where(q => q.LessonId == lessonId)
                .Select(x => mapper.Map<TestQuestionDto>(x))).ToListAsync();

            if (!lessonQuestions.Any())
            {
                return BaseResult<LessonPracticeDto>.Failure((int)ErrorCodes.QuestionsNotFound, ErrorMessage.QuestionsNotFound);
            }

            return BaseResult<LessonPracticeDto>.Success(new LessonPracticeDto()
            {
                LessonId = lesson.Id,
                LessonType = lesson.LessonType,
                Questions = lessonQuestions
            });
        }

        #region Pass Lesson 

        public async Task<BaseResult<PracticeCorrectAnswersDto>> PassLessonAsync(PracticeUserAnswersDto dto, Guid userId)
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
                    .Include(q => q.Lesson)
                    .Where(q => q.LessonId == lesson.Id)
                    .OrderBy(x => x.Number)
                    .ToListAsync();

            if (!questions.Any() || questions.Count != dto.UserAnswerDtos.Count)
            {
                return BaseResult<PracticeCorrectAnswersDto>.Failure((int)ErrorCodes.LessonQuestionsNotFound,
                    ErrorMessage.LessonQuestionsNotFound);
            }

            var correctAnswers = CheckUserAnswers(dto.UserAnswerDtos, questions, out float userGrade);

            return BaseResult<PracticeCorrectAnswersDto>.Success(new PracticeCorrectAnswersDto()
            {
                LessonId = lesson.Id,
                QuestionCorrectAnswers = correctAnswers
            });
        }

        //To Do сделать так, чтобы пользователю после практики показывалось количество баллов за ответ
        //To Do оптимизировать код
        private List<ICorrectAnswerDto> CheckUserAnswers(List<IUserAnswerDto> userAnswers, List<Question> lessonQuestions, out float userGrade)
        {
            var correctAnswers = new List<ICorrectAnswerDto>();

            userGrade = 0;

            if (userAnswers.Count != lessonQuestions.Count)
            {
                return new List<ICorrectAnswerDto>();
            }

            for (int i = 0; i < userAnswers.Count && i < lessonQuestions.Count; i++)
            {
                //Если вопрос тестовый
                if (lessonQuestions[i] is TestQuestion && userAnswers[i] is TestQuestionUserAnswerDto testUserAnswer)
                {
                    var correctTestVariant = unitOfWork.TestVariants.GetAll()
                        .FirstOrDefault(x => x.QuestionId == lessonQuestions[i].Id && x.IsRight);

                    var testQuestionCorrectAnswerDto = new TestQuestionCorrectAnswerDto()
                    {
                        Id = lessonQuestions[i].Id,
                        CorrectAnswer = correctTestVariant.Content,
                    };

                    if (testUserAnswer.UserAnswerNumberOfVariant == correctTestVariant.VariantNumber)
                    {
                        userGrade++;

                        testQuestionCorrectAnswerDto.AnswerCorrectness = true;
                    }
                    else
                    {
                        testQuestionCorrectAnswerDto.AnswerCorrectness = false;
                    }

                    correctAnswers.Add(testQuestionCorrectAnswerDto);
                }
                //Если вопрос открытый
                else if (lessonQuestions[i] is OpenQuestion && userAnswers[i] is OpenQuestionUserAnswerDto openQuestionUserAnswerDto)
                {
                    var correctAnswersOnOpenQuestions = unitOfWork.OpenQuestionAnswerVariants.GetAll()
                        .Where(x => x.QuestionId == lessonQuestions[i].Id).ToList();

                    var openQuestionCorrectAnswerDto = new OpenQuestionCorrectAnswerDto()
                    {
                        Id = lessonQuestions[i].Id,
                        CorrectAnswer = correctAnswersOnOpenQuestions.FirstOrDefault().OpenQuestionCorrectAnswer
                    };

                    if (correctAnswersOnOpenQuestions.Any(x => x.OpenQuestionCorrectAnswer == openQuestionUserAnswerDto.UserAnswer))
                    {
                        userGrade = userGrade + 2;

                        openQuestionCorrectAnswerDto.AnswerCorrectness = true;
                    }
                    else
                    {
                        openQuestionCorrectAnswerDto.AnswerCorrectness = false;
                    }

                    correctAnswers.Add(openQuestionCorrectAnswerDto);
                } 
                else if (lessonQuestions[i] is PracticalQuestion practicalQuestion && userAnswers[i] is PracticalQuestionUserAnswerDto practicalQuestionUserAnswerDto)
                {

                    var practicalQuestionCorrectAnswerDto = new PracticalQuestionCorrectAnswerDto()
                    {
                        Id = lessonQuestions[i].Id,
                        CorrectAnswer = practicalQuestion.RightQueryCode
                    };

                    var remarks = new List<string>();
                    try
                    {
                        var userResult = SqlHelper.ExecuteQuery(practicalQuestionUserAnswerDto.UserCodeAnswer);
                        var rightResult = SqlHelper.ExecuteQuery(practicalQuestion.RightQueryCode);

                        DataTableComparer comparer = new DataTableComparer();
                        int result = comparer.Compare(userResult, rightResult);

                        if (result == 0)
                        {
                            practicalQuestionCorrectAnswerDto.AnswerCorrectness = true;

                            userGrade = userGrade + 5.75f;

                            if (userResult != null)
                            {
                                practicalQuestionCorrectAnswerDto.QueryResult = userResult;
                            }

                            correctAnswers.Add(practicalQuestionCorrectAnswerDto);
                        }
                        else
                        {
                            ParseAnswer(practicalQuestionUserAnswerDto.UserCodeAnswer.ToLower(), practicalQuestion.Id, out float practicalQuestionGrade, out remarks);

                            practicalQuestionCorrectAnswerDto.AnswerCorrectness = false;
                            practicalQuestionCorrectAnswerDto.Remarks = remarks;

                            userGrade = userGrade + practicalQuestionGrade;

                            correctAnswers.Add(practicalQuestionCorrectAnswerDto);
                        }

                    }
                    catch (Exception)
                    {
                        if (practicalQuestionUserAnswerDto.UserCodeAnswer != null)
                        {
                            ParseAnswer(practicalQuestionUserAnswerDto.UserCodeAnswer.ToLower(), practicalQuestion.Id, out float practicalQuestionGrade, out remarks);

                            practicalQuestionCorrectAnswerDto.AnswerCorrectness = false;
                            practicalQuestionCorrectAnswerDto.Remarks = remarks;

                            correctAnswers.Add(practicalQuestionCorrectAnswerDto);
                        }
                    }
                }
            }

            return correctAnswers;
        }

        public void ParseAnswer(string sqlQuery, int questionId, out float grade, out List<string> remarks)
        {
            grade = 0f;
            remarks = new List<string>();
            if (sqlQuery == null)
                return;

            var getQuestionKeywords = unitOfWork.QueryWords.GetAll()
                .Where(x => x.QuestionId == questionId)
                .OrderBy(x => x.Number)
                .Include(x => x.Keyword)
                .Select(x => x.Keyword.Word).ToList();

            AdjacencyGraph<string, Edge<string>> graph = CreateGraph(getQuestionKeywords);
            float gradeForeachCategory = (float)Math.Round(5.75f / (getQuestionKeywords.Count + 2), 2);

            var words = sqlQuery.Split(new[] { ' ', ',', '.', '(', ')', ';' }, StringSplitOptions.RemoveEmptyEntries);
            var keywordIndexes = new List<int>();

            bool isFirstIteration = true;
            foreach (var edge in graph.Edges)
            {
                if (isFirstIteration)
                {
                    keywordIndexes.Add(sqlQuery.IndexOf(edge.Source));
                    keywordIndexes.Add(sqlQuery.IndexOf(edge.Target));
                    isFirstIteration = false;
                }
                else
                {
                    keywordIndexes.Add(sqlQuery.IndexOf(edge.Target));
                }
            }

            foreach (var keyword in graph.Vertices)
            {
                keywordIndexes.Add(sqlQuery.IndexOf(keyword));
            }

            if (keywordIndexes.SequenceEqual(keywordIndexes.OrderBy(x => x)))
            {
                grade += gradeForeachCategory;
            }
            else
            {
                remarks.Add("Служебные слова расположены не в правильном порядке");
            }

            foreach (var keyword in graph.Vertices)
            {
                if (words.Contains(keyword))
                {
                    grade += gradeForeachCategory;
                }
                else
                {
                    remarks.Add($"Вы не добавили с свой запрос служебное слово {keyword.ToUpper()}");
                }
            }

            grade = (float)Math.Round(grade, 2);
        }

        private AdjacencyGraph<string, Edge<string>> CreateGraph(IEnumerable<string> keywords)
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();
            foreach (var keyword in keywords)
            {
                graph.AddVertex(keyword);
            }
            for (int i = 0; i < keywords.Count() - 1; i++)
            {
                graph.AddEdge(new Edge<string>(keywords.ElementAt(i), keywords.ElementAt(i + 1)));
            }
            return graph;
        }

        #endregion

    }
}