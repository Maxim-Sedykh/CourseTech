using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Lesson
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string DisplayQuestion { get; set; }

        public QuestionType QuestionType { get; set; }

        public List<TestVariant> VariantsOfAnswer { get; set; }

        public DataTable QueryResult { get; set; }

        public List<string> Remarks { get; set; }

        public string RightPageAnswer { get; set; }

        public List<string> InnerAnswers { get; set; }

        public string UserAnswer { get; set; }

        public bool AnswerCorrectness { get; set; }
    }
}
