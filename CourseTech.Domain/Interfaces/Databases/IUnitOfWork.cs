using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CourseTech.Domain.Interfaces.Databases;

public interface IUnitOfWork : IStateSaveChanges
{
    Task<IDbContextTransaction> BeginTransactionAsync();

    IBaseRepository<User> Users { get; set; }

    IBaseRepository<Role> Roles { get; set; }

    IBaseRepository<UserToken> UserTokens { get; set; }

    IBaseRepository<UserProfile> UserProfiles { get; set; }

    IBaseRepository<Review> Reviews { get; set; }

    IBaseRepository<Lesson> Lessons { get; set; }

    IBaseRepository<UserRole> UserRoles { get; set; }

    IBaseRepository<TestQuestion> TestQuestions { get; set; }

    IBaseRepository<OpenQuestion> OpenQuestions { get; set; }

    IBaseRepository<PracticalQuestion> PracticalQuestions { get; set; }

    IBaseRepository<TestVariant> TestVariants { get; set; }

    IBaseRepository<LessonRecord> LessonRecords { get; set; }

    IBaseRepository<QueryWord> QueryWords { get; set; }

    IBaseRepository<Question> Questions { get; set; }

    IBaseRepository<OpenQuestionAnswerVariant> OpenQuestionAnswerVariants { get; set; }

    IBaseRepository<Keyword> Keywords { get; set; }
}
