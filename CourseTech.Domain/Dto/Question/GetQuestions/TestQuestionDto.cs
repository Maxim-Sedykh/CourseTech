using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json.Serialization;

namespace CourseTech.Domain.Dto.Question.Get
{
    [JsonDerivedType(typeof(IQuestionDto), typeDiscriminator: "base")]
    public class TestQuestionDto : IQuestionDto
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string DisplayQuestion { get; set; }

        public List<TestVariantDto> TestVariants { get; set; }
    }
}
