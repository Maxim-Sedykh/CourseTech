using CourseTech.Application.CQRS.Queries.Entities.UserQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.EntityQueryHandlers.UserQueryHandlers;

public class GetUserByIdHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserByIdQuery, User>
{
    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetAll()
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
    }
}
