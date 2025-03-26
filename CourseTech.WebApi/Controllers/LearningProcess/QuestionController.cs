using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.LearningProcess;

//[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class QuestionController(IQuestionService questionService) : BaseApiController
{
    [HttpGet(RouteConstants.GetLessonQuestions)]
    public async Task<ActionResult<DataResult<LessonPracticeDto>>> GetLessonQuestionsAsync(int lessonId)
    {
        var response = await questionService.GetLessonQuestionsAsync(lessonId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost(RouteConstants.PassLessonQuestions)]
    public async Task<ActionResult<DataResult<PracticeCorrectAnswersDto>>> PassLessonQuestionsAsync([FromBody] PracticeUserAnswersDto dto)
    {
        //var response = await questionService.PassLessonQuestionsAsync(dto, AuthorizedUserId);
        var response = await questionService.PassLessonQuestionsAsync(dto, new Guid("315B2EE7-BA7D-42D5-F1AA-08DD64A562B0"));
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
