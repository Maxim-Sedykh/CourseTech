using CourseTech.Application.Services;
using CourseTech.Application.Validations.FluentValidations.Lesson;
using CourseTech.Application.Validations.FluentValidations.Review;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController(ILessonService lessonService, LessonLectureValidator lessonLectureValidator) : ControllerBase
    {
        [HttpPut]
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

        [HttpGet()]
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

        [HttpGet()]
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

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserLessonsDto>>> GetLessonsForUserAsync()
        {
            var response = await lessonService.GetLessonsForUserAsync(new Guid(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).ToString()));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
