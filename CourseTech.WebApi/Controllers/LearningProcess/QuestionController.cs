﻿using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.LearningProcess;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class QuestionController(IQuestionService questionService) : BaseApiController
{
    [HttpPost(RouteConstants.GetLessonQuestions)]
    public async Task<ActionResult<DataResult<LessonPracticeDto>>> GetLessonQuestionsAsync(GetLessonQuestionsRequest request)
    {
        var response = await questionService.GetLessonQuestionsAsync(request.LessonId, request.IsDemoMode);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost(RouteConstants.PassLessonQuestions)]
    public async Task<ActionResult<DataResult<PracticeCorrectAnswersDto>>> PassLessonQuestionsAsync(PracticeUserAnswersDto dto)
    {
        var response = await questionService.PassLessonQuestionsAsync(dto, AuthorizedUserId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    public class GetLessonQuestionsRequest
    {
        public int LessonId { get; set; }
        public bool IsDemoMode { get; set; }
    }
}
