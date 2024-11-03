using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Keyword
{
    /// <summary>
    /// Модель данных для ключевого слова запроса.
    /// </summary>
    public class KeywordDto
    {
        public int QuestionId { get; set; }

        public string Keyword { get; set; }
    }
}
