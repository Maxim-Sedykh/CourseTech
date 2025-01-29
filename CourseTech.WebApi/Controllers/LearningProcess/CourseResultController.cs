using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.LearningProcess
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseResultController(ICourseResultService courseResultService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetCouserResult)]
        public async Task<ActionResult<DataResult<CourseResultDto>>> GetCourseResult()
        {
            var response = await courseResultService.GetCourseResultAsync(AuthorizedUserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetUserAnalys)]
        public async Task<ActionResult<DataResult<UserAnalysDto>>> GetUserAnalys()
        {
            var response = await courseResultService.GetUserAnalys(AuthorizedUserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
