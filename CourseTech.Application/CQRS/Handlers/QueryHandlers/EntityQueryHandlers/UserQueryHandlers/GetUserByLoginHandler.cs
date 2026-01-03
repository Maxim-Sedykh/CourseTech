using CourseTech.Application.CQRS.Queries.Entities.UserQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.EntityQueryHandlers.UserQueryHandlers;

public class GetUserByLoginHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserByLoginQuery, User>
{
    public async Task<User> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);
    }
}
