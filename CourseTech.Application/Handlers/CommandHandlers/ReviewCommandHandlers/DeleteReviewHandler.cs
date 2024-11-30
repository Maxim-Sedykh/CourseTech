using CourseTech.Application.Commands.Reviews;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.CommandHandlers.ReviewCommandHandlers
{
    public class DeleteReviewHandler(IBaseRepository<Review> reviewRepository) : IRequestHandler<DeleteReviewCommand>
    {
        public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            reviewRepository.Remove(request.Review);

            await reviewRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
