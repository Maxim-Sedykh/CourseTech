using CourseTech.Domain.Dto.User;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.UserDtoQueries;

/// <summary>
/// Обновить пользователя
/// </summary>
/// <param name="UserId"></param>
public record GetUpdateUserDtoByUserIdQuery(Guid UserId) : IRequest<UpdateUserDto>;
