﻿namespace CourseTech.Domain.Dto.FinalResult;

/// <summary>
/// Модель данных для показа результата прохождения курса.
/// </summary>
public class CourseResultDto
{
    public Guid UserId { get; set; }

    public string Login { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public float CurrentGrade { get; set; }

    public string Analys { get; set; }
}
