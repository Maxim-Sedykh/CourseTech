using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.RoleQueries;

/// <summary>
/// Получить роль по её названию.
/// </summary>
/// <param name="Name"></param>
public record GetRoleByNameQuery(string Name) : IRequest<Role>;
