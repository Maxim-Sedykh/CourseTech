using CourseTech.Application.CQRS.Commands.ReviewCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.ReviewCommandHandlers;

public class CreateReviewHandler(IBaseRepository<Review> reviewRepository) : IRequestHandler<CreateReviewCommand>
{
    public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = new Review()
        {
            UserId = request.UserId,
            ReviewText = request.ReviewText
        };

        await reviewRepository.CreateAsync(review);
    }
}
