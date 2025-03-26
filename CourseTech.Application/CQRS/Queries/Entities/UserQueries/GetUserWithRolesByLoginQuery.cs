using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.UserQueries;

/// <summary>
/// Получение пользователя с ролями по логину пользователя.
/// </summary>
/// <param name="Login"></param>
public record GetUserWithRolesByLoginQuery(string Login) : IRequest<User>;
