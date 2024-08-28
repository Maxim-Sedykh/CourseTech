using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Helpers;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using QuickGraph;

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

            var lessonNames = await unitOfWork.Lessons.GetAll()
                .Select(x => mapper.Map<LessonDto>(x))
                .ToListAsync();

            if (lessonNames is null)
            {
                return BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
            }

            return BaseResult<UserLessonsDto>.Success(new UserLessonsDto()
            {
                LessonNames = lessonNames,
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

        public async Task<BaseResult<LessonPassDto>> PassLessonAsync(LessonPassDto dto, Guid userId)
        {
            var profile = await unitOfWork.UserProfiles.GetAll().FirstOrDefaultAsync(x => x.UserId == userId);
            if (profile == null)
            {
                return BaseResult<LessonPassDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            Tuple<float, List<bool>> tasksEvaluations = CheckTasks(dto); //Проверка ответов пользователя
            if (dto.LessonId > profile.LessonsCompleted)
            {
                profile.CurrentGrade = +tasksEvaluations.Item1;
                profile.LessonsCompleted++;

                unitOfWork.UserProfiles.Update(profile);

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.LessonRecords.CreateAsync(new LessonRecord
                {
                    LessonId = dto.LessonId,
                    UserId = profile.UserId,
                    Mark = tasksEvaluations.Item1
                });
            }

            for (int i = 0; i < dto.Questions.Count; i++)
            {
                dto.Questions[i].AnswerCorrectness = tasksEvaluations.Item2[i];
            }

            return BaseResult<LessonPassDto>.Success(dto);
        }

        public async Task<BaseResult<LessonPassDto>> GetLessonQuestionsAsync(int lessonId)
        {
            var lesson = await unitOfWork.Lessons.GetAll().FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson is null)
            {
                return BaseResult<LessonPassDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            var lessonPassDto = GetLessonQuestions(lesson);

            if (lessonPassDto is null)
            {
                return BaseResult<LessonPassDto>.Failure((int)ErrorCodes.LessonNotFound, ErrorMessage.LessonNotFound);
            }

            return BaseResult<LessonPassDto>.Failure((int)ErrorCodes.QuestionsNotFound, ErrorMessage.QuestionsNotFound);
        }

        private LessonPassDto GetLessonQuestions(Lesson currentLesson)
        {
            var lessonQuestions = unitOfWork.Questions.GetAll().Where(question => question.LessonId == currentLesson.Id).ToList();
            var lessonTestVariants = unitOfWork.Questions.GetAll().Where(question => question.LessonId == currentLesson.Id && question.Type == QuestionType.Test)
                                                    .Join(unitOfWork.TestVariants.GetAll(), question => question.Id, testVariant => testVariant.QuestionId, (question, testVariant) => testVariant)
                                                    .ToList();
            var questionViewModels = new List<QuestionDto>();
            for (int i = 0; i < lessonQuestions.Count; i++)
            {
                if (i > 0 && lessonQuestions[i - 1].Number == lessonQuestions[i].Number)
                {
                    questionViewModels.Last().InnerAnswers.Add(lessonQuestions[i].Answer);
                    continue;
                }
                else
                {
                    questionViewModels.Add(new QuestionDto
                    {
                        Id = lessonQuestions[i].Id,
                        Number = lessonQuestions[i].Number,
                        DisplayQuestion = lessonQuestions[i].DisplayQuestion,
                        QuestionType = lessonQuestions[i].Type,
                        VariantsOfAnswer = lessonQuestions[i].Type == QuestionType.Test ? (from testVariant in lessonTestVariants
                                                                                           where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                           select testVariant).ToList() : null,
                        InnerAnswers = new List<string> { lessonQuestions[i].Answer },
                        RightPageAnswer = (lessonQuestions[i].Type == QuestionType.Open) || (lessonQuestions[i].Type == QuestionType.Practical) ? lessonQuestions[i].Answer : (from testVariant in lessonTestVariants
                                                                                                                                                                               where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                                                                                                               where testVariant.IsRight
                                                                                                                                                                               select testVariant.Content).First(),
                    });
                }
            }
            return new LessonPassDto
            {
                LessonId = currentLesson.Id,
                Questions = questionViewModels,
                LessonType = currentLesson.LessonType
            };
        }

        private Tuple<float, List<bool>> CheckTasks(LessonPassDto model) // Проверка заданий тестового и открытого типа
        {
            float grade = 0;
            List<bool> tasksCorrectness = new List<bool>();

            foreach (var question in model.Questions)
            {
                bool isAnswerCorrect = false;
                foreach (var answer in question.InnerAnswers)
                {
                    if (question.QuestionType == QuestionType.Practical)
                    {
                        var remarks = new List<string>();
                        try
                        {
                            var userResult = SqlHelper.ExecuteQuery(question.UserAnswer);
                            var rightResult = SqlHelper.ExecuteQuery(question.InnerAnswers.First());

                            DataTableComparer comparer = new DataTableComparer();
                            int result = comparer.Compare(userResult, rightResult);

                            if (result == 0)
                            {
                                isAnswerCorrect = true;
                                grade = +5.75f;
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
                            if (question.QuestionType == QuestionType.Test)
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

    }
}