using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseTech.Domain.Dto.Review
{
    public class ReviewDto
    {
        public long Id { get; set; }

        public Guid UserId { get; set; }

        public string Login { get; set; }

        public string ReviewText { get; set; }

        public string CreatedAt { get; set; }
    }
}
