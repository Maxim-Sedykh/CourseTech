using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.Review;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReviewController(IReviewService reviewService, CreateReviewValidator createReviewValidator) : BaseApiController
    {

        [HttpPost(RouteConstants.CreateReview)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult>> CreateReviewAsync([FromBody] CreateReviewDto dto)
        {
            var validationResult = await createReviewValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await reviewService.CreateReviewAsync(dto, UserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete(RouteConstants.DeleteReview)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult>> DeleteReviewAsync(long id)
        {
            var response = await reviewService.DeleteReview(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetReviews)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<ReviewDto>>> GetReviewsAsync()
        {
            var response = await reviewService.GetReviewsAsync();
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetUserReviews)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<ReviewDto>>> GetUserReviews(Guid userId)
        {
            var response = await reviewService.GetUserReviews(userId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
