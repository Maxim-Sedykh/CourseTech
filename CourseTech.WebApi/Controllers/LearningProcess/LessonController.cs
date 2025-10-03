using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.Lesson;
using CourseTech.Domain.Constants;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.Lesson.Info;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using CourseTech.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.LearningProcess;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LessonController(ILessonService lessonService, LessonLectureValidator lessonLectureValidator) : BaseApiController
{
    [AllowRoles(Roles.Moderator, Roles.Admin)]
    [HttpPut(RouteConstants.UpdateLessonLecture)]
    public async Task<ActionResult<DataResult<LessonLectureDto>>> UpdateLessonLectureAsync([FromBody] LessonLectureDto dto)
    {
        var validationResult = await lessonLectureValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var response = await lessonService.UpdateLessonLectureAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet(RouteConstants.GetLessonLecture)]
    public async Task<ActionResult<DataResult<LessonLectureDto>>> GetLessonLectureAsync(int lessonId)
    {
        var response = await lessonService.GetLessonLectureAsync(lessonId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [AllowAnonymous]
    [HttpGet(RouteConstants.GetLessonNames)]
    public async Task<ActionResult<CollectionResult<LessonNameDto>>> GetLessonNamesAsync()
    {
        var response = await lessonService.GetLessonNamesAsync();
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet(RouteConstants.GetLessonsForUser)]
    public async Task<ActionResult<DataResult<UserLessonsDto>>> GetLessonsForUserAsync()
    {
        var response = await lessonService.GetLessonsForUserAsync(AuthorizedUserId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
