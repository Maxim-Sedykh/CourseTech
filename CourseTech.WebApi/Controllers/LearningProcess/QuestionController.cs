﻿using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
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
    public class QuestionController(IQuestionService questionService) : BaseApiController
    {
        [HttpGet(RouteConstants.GetLessonQuestions)]
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
        public async Task<ActionResult<BaseResult<PracticeCorrectAnswersDto>>> PassLessonQuestionsAsync([FromBody] PracticeUserAnswersDto dto)
        {
            var response = await questionService.PassLessonQuestionsAsync(dto, new Guid("A002B50D-52B5-4D02-1DF0-08DD061EC8FC"));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
