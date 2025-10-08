using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities.UserRelated;
using MediatR;

namespace CourseTech.Application.Commands.UserCommands;

/// <summary>
/// Обновление информации о пользователе.
/// </summary>
/// <param name="UpdateUserDto"></param>
/// <param name="User"></param>
public record UpdateUserCommand(UpdateUserDto UpdateUserDto, User User) : IRequest;
