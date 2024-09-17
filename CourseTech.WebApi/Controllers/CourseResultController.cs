using Asp.Versioning;
using CourseTech.Application.Services;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseResultController(ICourseResultService courseResultService) : ControllerBase
    {
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CourseResultDto>>> GetCouserResult()
        {
            var response = await courseResultService.GetCourseResultAsync(new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).ToString()));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("analys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserAnalysDto>>> GetUserAnalys()
        {
            var response = await courseResultService.GetUserAnalys(new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).ToString()));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
