using Asp.Versioning;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserProfileController(IUserProfileService userProfileService) : ControllerBase
    {
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserProfileDto>>> GetUserProfileAsync()
        {
            var response = await userProfileService.GetUserProfileAsync(new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).ToString()));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult>> UpdateUserProfileAsync([FromBody] UserProfileDto dto)
        {
            var response = await userProfileService.UpdateUserProfileAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
