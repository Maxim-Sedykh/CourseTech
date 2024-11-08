using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.DAL.Configurations
{
    /// <summary>
    /// Конфигурация сущности "Вопрос практического типа" (настройка таблицы в БД)
    /// </summary>
    public class PracticalQuestionConfiguration : IEntityTypeConfiguration<PracticalQuestion>
    {
        public void Configure(EntityTypeBuilder<PracticalQuestion> builder)
        {
            builder.HasBaseType<Question>();

            builder.Property(q => q.CorrectQueryCode).IsRequired();
        }
    }
}
