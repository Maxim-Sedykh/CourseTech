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
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LessonRecordController(ILessonRecordService lessonRecordService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetLessonsRecords)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<LessonRecordDto>>> GetLessonsRecords()
        {
            var response = await lessonRecordService.GetLessonRecordsAsync(UserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
