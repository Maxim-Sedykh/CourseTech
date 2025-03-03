﻿using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Question.CheckQuestions
{
    /// <summary>
    /// Модель данных для хранения данных, которые нужны для проверки правильности вопроса практического типа.
    /// </summary>
    public class PracticalQuestionCheckingDto : ICheckQuestionDto
    {
        public int QuestionId { get; set; }

        public string CorrectQueryCode { get; set; }
    }
}
