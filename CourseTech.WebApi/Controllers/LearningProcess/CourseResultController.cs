using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.CourseResult;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.LearningProcess
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseResultController(ICourseResultService courseResultService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetCouserResult)]
        public async Task<ActionResult<DataResult<CourseResultDto>>> GetCourseResult()
        {
            //var response = await courseResultService.GetCourseResultAsync(AuthorizedUserId);
            var response = await courseResultService.GetCourseResultAsync(new Guid("3C3AF900-4B48-481C-B58D-08DD0BB4EFCA"));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetUserAnalys)]
        public async Task<ActionResult<DataResult<UserAnalysDto>>> GetUserAnalys()
        {
            //var response = await courseResultService.GetUserAnalys(AuthorizedUserId);
            var response = await courseResultService.GetUserAnalys(new Guid("3C3AF900-4B48-481C-B58D-08DD0BB4EFCA"));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
