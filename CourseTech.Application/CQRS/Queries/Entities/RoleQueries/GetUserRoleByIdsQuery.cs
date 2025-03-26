using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.RoleQueries;

/// <summary>
/// Получение сущности "UserRole" по его идентификаторам.
/// </summary>
/// <param name="RoleId"></param>
/// <param name="UserId"></param>
public record GetUserRoleByIdsQuery(long RoleId, Guid UserId) : IRequest<UserRole>;
