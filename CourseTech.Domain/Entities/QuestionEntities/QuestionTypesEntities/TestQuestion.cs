using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities
{
    public class TestQuestion : Question
    {
        public List<TestVariant> TestVariants { get; set; }
    }
}
