using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.RoleCommands;

/// <summary>
/// Удаление роли для пользователя.
/// </summary>
/// <param name="UserRole"></param>
public record DeleteUserRoleCommand(UserRole UserRole) : IRequest;
