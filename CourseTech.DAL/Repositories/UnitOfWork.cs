using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CourseTech.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CourseDbContext _context;

    public IBaseRepository<User> Users { get; set; }

    public IBaseRepository<Review> Reviews { get; set; }

    public IBaseRepository<UserProfile> UserProfiles { get; set; }

    public IBaseRepository<Role> Roles { get; set; }

    public IBaseRepository<UserToken> UserTokens { get; set; }

    public IBaseRepository<Lesson> Lessons { get; set; }

    public IBaseRepository<UserRole> UserRoles { get; set; }

    public IBaseRepository<LessonRecord> LessonRecords { get; set; }

    public IBaseRepository<Question> Questions { get; set; }

    public IBaseRepository<TestVariant> TestVariants { get; set; }

    public IBaseRepository<QueryWord> QueryWords { get; set; }

    public IBaseRepository<TestQuestion> TestQuestions { get; set; }

    public IBaseRepository<OpenQuestion> OpenQuestions { get; set; }

    public IBaseRepository<PracticalQuestion> PracticalQuestions { get; set; }

    public IBaseRepository<OpenQuestionAnswerVariant> OpenQuestionAnswerVariants { get; set; }

    public UnitOfWork(CourseDbContext context, IBaseRepository<User> users, IBaseRepository<Review> reviews)
    {
        _context = context;
        Users = users;
        Reviews = reviews;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
