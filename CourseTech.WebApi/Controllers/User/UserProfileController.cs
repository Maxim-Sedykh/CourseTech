using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserProfileController(IUserProfileService userProfileService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetUserProfile)]
        public async Task<ActionResult<DataResult<UserProfileDto>>> GetUserProfileAsync()
        {
            var response = await userProfileService.GetUserProfileAsync(AuthorizedUserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(RouteConstants.UpdateUserProfile)]
        public async Task<ActionResult<BaseResult>> UpdateUserProfileAsync([FromBody] UpdateUserProfileDto dto)
        {
            var response = await userProfileService.UpdateUserProfileAsync(dto, AuthorizedUserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
