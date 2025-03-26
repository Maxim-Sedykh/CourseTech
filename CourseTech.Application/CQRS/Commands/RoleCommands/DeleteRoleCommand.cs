using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.RoleCommands;

/// <summary>
/// Удаление роли
/// </summary>
/// <param name="Role"></param>
public record DeleteRoleCommand(Role Role) : IRequest;
