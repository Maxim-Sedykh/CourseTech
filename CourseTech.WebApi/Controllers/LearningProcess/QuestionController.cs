using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers.LearningProcess
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class QuestionController(IQuestionService questionService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetLessonQuestions)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<LessonPracticeDto>>> GetLessonQuestionsAsync(int lessonId)
        {
            var response = await questionService.GetLessonQuestionsAsync(lessonId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost(RouteConstants.PassLessonQuestions)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<PracticeCorrectAnswersDto>>> PassLessonQuestionsAsync([FromBody] PracticeUserAnswersDto dto)
        {
            var response = await questionService.PassLessonQuestionsAsync(dto, new Guid("87CCB692-B6EB-44B2-5354-08DCDF8CC4AF"));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
