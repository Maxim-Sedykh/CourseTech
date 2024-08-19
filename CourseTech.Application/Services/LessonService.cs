using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using QuickGraph;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.DAL.SqlHelper;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Domain.ViewModels.Teaching;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CourseTech.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly IBaseRepository<UserProfile> _userProfileRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;
        private readonly IBaseRepository<Lesson> _lessonRepository;
        private readonly IBaseRepository<Question> _questionRepository;
        private readonly IBaseRepository<TestVariant> _testVariantRepository;
        private readonly IBaseRepository<QueryWord> _queryWordsRepository;

        public LessonService(IBaseRepository<LessonRecord> lessonRecordRepository,
            IBaseRepository<Lesson> lessonRepository, IBaseRepository<UserProfile> userProfileRepository,
            IBaseRepository<Question> questionRepository, IBaseRepository<TestVariant> pageAnswerRepository,
            IBaseRepository<QueryWord> queryWordsRepository)
        {
            _lessonRecordRepository = lessonRecordRepository;
            _lessonRepository = lessonRepository;
            _userProfileRepository = userProfileRepository;
            _questionRepository = questionRepository;
            _testVariantRepository = pageAnswerRepository;
            _queryWordsRepository = queryWordsRepository;
        }

        public IBaseResponse<LessonLectureViewModel> GetLecture(byte lessonId)
        {
            try
            {
                var lesson = _lessonRepository.GetAll().FirstOrDefault(x => x.Id == lessonId);
                if (lesson is null)
                {
                    return new BaseResponse<LessonLectureViewModel>()
                    {
                        StatusCode = StatusCode.LessonNotFound,
                        Description = "Урок не найден"
                    };
                }

                return new BaseResponse<LessonLectureViewModel>()
                {
                    Data = new LessonLectureViewModel()
                    {
                        Id = lessonId,
                        LessonName = lesson.Name,
                        LessonMarkup = new HtmlString($"{lesson.LectureMarkup}")
                    },
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LessonLectureViewModel>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public IBaseResponse<LessonPassViewModel> GetQuestions(byte lessonId)
        {
            try
            {
                var currentLesson = _lessonRepository.GetAll().FirstOrDefault(x => x.Id == lessonId);
                if (currentLesson is null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.LessonNotFound,
                        Description = "Урок не найден"
                    };
                }

                var response = GetLessonQuestions(currentLesson);

                if (response is null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.TasksDataNotFound,
                        Description = "Данные заданий не найдены"
                    };
                }
                return new BaseResponse<LessonPassViewModel>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LessonPassViewModel>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel model, string userLogin) // Прохождение практической части уроков
        {
            try
            {
                var profile = await _userProfileRepository.GetAll().FirstOrDefaultAsync(x => x.User.Login == userLogin);
                if (profile == null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                Tuple<float, List<bool>> tasksEvaluations = CheckTasks(model); //Проверка ответов пользователя
                if (model.LessonId > profile.LessonsCompleted)
                {
                    profile.CurrentGrade = +tasksEvaluations.Item1;
                    profile.LessonsCompleted++;

                    await _userProfileRepository.Update(profile);
                    await _lessonRecordRepository.Create(new LessonRecord
                    {
                        LessonId = model.LessonId,
                        UserId = profile.Id,
                        Mark = tasksEvaluations.Item1
                    });
                }

                for (int i = 0; i < model.Questions.Count; i++)
                {
                    model.Questions[i].AnswerCorrectness = tasksEvaluations.Item2[i];
                }

                return new BaseResponse<LessonPassViewModel>()
                {
                    Data = model,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LessonPassViewModel>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        private LessonPassViewModel GetLessonQuestions(Lesson currentLesson)
        {
            var lessonQuestions = _questionRepository.GetAll().Where(question => question.LessonId == currentLesson.Id).ToList();
            var lessonTestVariants = _questionRepository.GetAll().Where(question => question.LessonId == currentLesson.Id && question.Type == TaskType.Test)
                                                    .Join(_testVariantRepository.GetAll(), question => question.Id, testVariant => testVariant.QuestionId, (question, testVariant) => testVariant)
                                                    .ToList();
            var questionViewModels = new List<QuestionViewModel>();
            for (int i = 0; i < lessonQuestions.Count; i++)
            {
                if (i > 0 && lessonQuestions[i - 1].Number == lessonQuestions[i].Number)
                {
                    questionViewModels.Last().InnerAnswers.Add(lessonQuestions[i].Answer);
                    continue;
                }
                else
                {
                    questionViewModels.Add(new QuestionViewModel
                    {
                        Id = lessonQuestions[i].Id,
                        Number = lessonQuestions[i].Number,
                        DisplayQuestion = lessonQuestions[i].DisplayQuestion,
                        QuestionType = lessonQuestions[i].Type,
                        VariantsOfAnswer = lessonQuestions[i].Type == TaskType.Test ? (from testVariant in lessonTestVariants
                                                                                       where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                       select testVariant).ToList() : null,
                        InnerAnswers = new List<string> { lessonQuestions[i].Answer },
                        RightPageAnswer = lessonQuestions[i].Type == TaskType.Open || lessonQuestions[i].Type == TaskType.Practical ? lessonQuestions[i].Answer : (from testVariant in lessonTestVariants
                                                                                                                                                                   where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                                                                                                   where testVariant.IsRight
                                                                                                                                                                   select testVariant.Content).First(),
                    });
                }
            }
            return new LessonPassViewModel
            {
                LessonId = currentLesson.Id,
                Questions = questionViewModels,
                LessonType = currentLesson.LessonType
            };
        }

        private async Task<Tuple<float, List<bool>>> CheckTasks(LessonPassViewModel model) // Проверка заданий тестового и открытого типа
        {
            float grade = 0;
            List<bool> tasksCorrectness = new List<bool>();

            foreach (var question in model.Questions)
            {
                bool isAnswerCorrect = false;
                foreach (var answer in question.InnerAnswers)
                {
                    if (question.QuestionType == TaskType.Practical)
                    {
                        var remarks = new List<string>();
                        try
                        {
                            var userResult = await SqlHelper.ExecuteQueryAsync(question.UserAnswer);
                            var rightResult = await SqlHelper.ExecuteQueryAsync(question.InnerAnswers.First());

                            DataTableComparer comparer = new DataTableComparer();
                            int result = comparer.Compare(userResult, rightResult);

                            if (result == 0)
                            {
                                isAnswerCorrect = true;
                                grade += 5.75f;
                                tasksCorrectness.Add(true);
                            }
                            else
                            {
                                ParseAnswer(question.UserAnswer.ToLower(), question.Id, out grade, out remarks);
                                question.Remarks = remarks;
                            }
                            if (userResult != null)
                            {
                                question.QueryResult = userResult;
                            }
                        }
                        catch (Exception)
                        {
                            if (question.UserAnswer != null)
                            {
                                ParseAnswer(question.UserAnswer.ToLower(), question.Id, out grade, out remarks);
                                question.Remarks = remarks;
                            }
                        }
                    }
                    else
                    {
                        if (answer == question.UserAnswer)
                        {
                            isAnswerCorrect = true;
                            tasksCorrectness.Add(true);
                            if (question.QuestionType == TaskType.Test)
                            {
                                grade += 1f;
                            }
                            else
                            {
                                grade += 2f;
                            }
                            break;
                        }
                    }
                }
                if (!isAnswerCorrect)
                {
                    tasksCorrectness.Add(false);
                }
            }
            return new Tuple<float, List<bool>>(grade, tasksCorrectness);
        }

        public void ParseAnswer(string sqlQuery, int questionId, out float grade, out List<string> remarks)
        {
            grade = 0f;
            remarks = new List<string>();
            if (sqlQuery == null)
                return;

            var getQuestionKeywords = _queryWordsRepository.GetAll()
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

        public async Task<IBaseResponse<LessonLectureViewModel>> SaveLectureMarkup(LessonContentViewModel model)
        {
            try
            {
                var currentLesson = _lessonRepository.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (currentLesson == null)
                {
                    return new BaseResponse<LessonLectureViewModel>()
                    {
                        StatusCode = StatusCode.LessonNotFound,
                        Description = "Урок не найден"
                    };
                }

                currentLesson.LectureMarkup = model.LessonMarkup;

                await _lessonRepository.Update(currentLesson);

                return new BaseResponse<LessonLectureViewModel>()
                {
                    Description = "Изменения сохранены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LessonLectureViewModel>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
    }
}