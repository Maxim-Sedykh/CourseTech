using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.UserQueries;

/// <summary>
/// Получение пользователя по его логину.
/// </summary>
/// <param name="Login"></param>
public record GetUserByLoginQuery(string Login) : IRequest<User>;
