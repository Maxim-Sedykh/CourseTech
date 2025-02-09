using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.LessonRecord;
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
    public class LessonRecordController(ILessonRecordService lessonRecordService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetLessonsRecords)]
        public async Task<ActionResult<CollectionResult<LessonRecordDto>>> GetLessonsRecords()
        {
            //var response = await lessonRecordService.GetUserLessonRecordsAsync(AuthorizedUserId);
            var response = await lessonRecordService.GetUserLessonRecordsAsync(new Guid("3C3AF900-4B48-481C-B58D-08DD0BB4EFCA"));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
