using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
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

    public IBaseRepository<OpenQuestionAnswer> OpenQuestionAnswers { get; set; }

    public IBaseRepository<Keyword> Keywords { get; set; }

    public UnitOfWork(CourseDbContext context,
        IBaseRepository<User> users,
        IBaseRepository<Review> reviews,
        IBaseRepository<UserProfile> userProfiles, 
        IBaseRepository<Role> roles, 
        IBaseRepository<UserToken> userTokens, 
        IBaseRepository<Lesson> lessons,
        IBaseRepository<UserRole> userRoles, 
        IBaseRepository<LessonRecord> lessonRecords, 
        IBaseRepository<Question> questions, 
        IBaseRepository<TestVariant> testVariants, 
        IBaseRepository<QueryWord> queryWords, 
        IBaseRepository<TestQuestion> testQuestions, 
        IBaseRepository<OpenQuestion> openQuestions, 
        IBaseRepository<PracticalQuestion> practicalQuestions, 
        IBaseRepository<OpenQuestionAnswer> openQuestionAnswers,
        IBaseRepository<Keyword> keywords)
    {
        _context = context;
        Users = users;
        Reviews = reviews;
        UserProfiles = userProfiles;
        Roles = roles;
        UserTokens = userTokens;
        Lessons = lessons;
        UserRoles = userRoles;
        LessonRecords = lessonRecords;
        Questions = questions;
        TestVariants = testVariants;
        QueryWords = queryWords;
        TestQuestions = testQuestions;
        OpenQuestions = openQuestions;
        PracticalQuestions = practicalQuestions;
        OpenQuestionAnswers = openQuestionAnswers;
        Keywords = keywords;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
