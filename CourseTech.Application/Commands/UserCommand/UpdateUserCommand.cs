using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.UserCommand
{
    public record UpdateUserCommand(UpdateUserDto UpdateUserDto, User User) : IRequest;
}
