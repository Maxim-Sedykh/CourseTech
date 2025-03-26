using CourseTech.Domain.Dto.User;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.UserDtoQueries;

/// <summary>
/// Получение всех пользователей в виде коллекции UserDto.
/// </summary>
public record GetUserDtosQuery : IRequest<UserDto[]>;
