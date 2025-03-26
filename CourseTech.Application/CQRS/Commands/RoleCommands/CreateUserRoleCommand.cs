using MediatR;

namespace CourseTech.Application.CQRS.Commands.RoleCommands;

/// <summary>
/// Добавление роли для пользователя
/// </summary>
/// <param name="RoleId"></param>
/// <param name="UserId"></param>
public record CreateUserRoleCommand(long RoleId, Guid UserId) : IRequest;
