using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.UserQueries;

/// <summary>
/// Получение пользователя с его токеном и ролями по его логину.
/// </summary>
/// <param name="Login"></param>
public record GetUserWithTokenAndRolesByLoginQuery(string Login) : IRequest<User>;
