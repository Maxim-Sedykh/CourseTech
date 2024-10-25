using Asp.Versioning;
using CourseTech.Application.Services;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers.LearningProcess
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseResultController(ICourseResultService courseResultService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetCouserResult)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CourseResultDto>>> GetCouserResult()
        {
            var response = await courseResultService.GetCourseResultAsync(UserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetUserAnalys)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserAnalysDto>>> GetUserAnalys()
        {
            var response = await courseResultService.GetUserAnalys(new Guid("87ccb692-b6eb-44b2-5354-08dcdf8cc4af"));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
