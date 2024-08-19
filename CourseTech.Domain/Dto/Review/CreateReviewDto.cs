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
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Введите отзыв")]
        [MinLength(10, ErrorMessage = "Длина отзыва должна быть больше десяти символов")]
        [MaxLength(200, ErrorMessage = "Длина отзыва должна быть меньше двухста символов")]
        public string ReviewText { get; set; }
    }
}
