using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseTech.Domain.Dto.Review
{
    /// <summary>
    /// Модель данных для создания отзыва пользователем.
    /// </summary>
    public class CreateReviewDto
    {
        public string ReviewText { get; set; }
    }
}
