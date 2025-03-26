using CourseTech.Domain.Dto.Role;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.RoleDtoQueries;

/// <summary>
/// Получение всех ролей в виде RoleDto
/// </summary>
public record GetRoleDtosQuery : IRequest<RoleDto[]>;
