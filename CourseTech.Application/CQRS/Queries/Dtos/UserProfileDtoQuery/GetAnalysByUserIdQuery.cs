using CourseTech.Domain.Dto.CourseResult;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.UserProfileDtoQuery;

/// <summary>
/// Получить анализ пользователя по его идентификатору.
/// </summary>
/// <param name="UserId"></param>
public record GetAnalysByUserIdQuery(Guid UserId) : IRequest<UserAnalysDto>;
