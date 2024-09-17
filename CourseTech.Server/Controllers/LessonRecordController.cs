using CourseTech.Application.Services;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.Api.Controllers
{
    [Authorize()]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LessonRecordController(ILessonRecordService lessonRecordService) : ControllerBase
    {
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<LessonRecordDto>>> GetLessonsForUserAsync()
        {
            var response = await lessonRecordService.GetLessonRecordsAsync(new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).ToString()));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
