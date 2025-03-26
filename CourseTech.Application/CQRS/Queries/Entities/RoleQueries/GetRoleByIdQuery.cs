using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.RoleQueries;

/// <summary>
/// Получение роли по её идентификатору.
/// </summary>
/// <param name="RoleId"></param>
public record GetRoleByIdQuery(long RoleId) : IRequest<Role>;
