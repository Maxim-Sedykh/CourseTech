﻿using CourseTech.Application.Commands.Reviews;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.ReviewHandlers
{
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
}