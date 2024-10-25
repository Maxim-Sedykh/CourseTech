using Asp.Versioning;
using CourseTech.Application.Services;
using CourseTech.Application.Validations.FluentValidations.Lesson;
using CourseTech.Application.Validations.FluentValidations.Review;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
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
    public class LessonController(ILessonService lessonService, LessonLectureValidator lessonLectureValidator) : BaseApiController
    {
        [HttpPut(RouteConstants.UpdateLessonLecture)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<LessonLectureDto>>> UpdateLessonLectureAsync([FromBody] LessonLectureDto dto)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<LessonLectureDto>>> GetLessonLectureAsync(int lessonId)
        {
            var response = await lessonService.GetLessonLectureAsync(lessonId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetLessonNames)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<LessonNameDto>>> GetLessonNamesAsync()
        {
            var response = await lessonService.GetLessonNamesAsync();
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetLessonsForUser)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserLessonsDto>>> GetLessonsForUserAsync()
        {
            var response = await lessonService.GetLessonsForUserAsync(UserId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
