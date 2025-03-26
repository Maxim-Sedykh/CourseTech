using MediatR;

namespace CourseTech.Application.CQRS.Commands.RoleCommands;

/// <summary>
/// Создание роли.
/// </summary>
/// <param name="RoleName"></param>
public record CreateRoleCommand(string RoleName) : IRequest;
