using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.LearningProcess;

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
        var response = await lessonRecordService.GetUserLessonRecordsAsync(new Guid("315B2EE7-BA7D-42D5-F1AA-08DD64A562B0"));
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
