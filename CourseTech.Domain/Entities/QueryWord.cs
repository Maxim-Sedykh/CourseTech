﻿using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class QueryWord : IEntityId<long>
{
    public long Id { get; set; }

    public int Number { get; set; }

    public int KeywordId { get; set; }

    public Keyword Keyword { get; set; }

    public int PracticalQuestionId { get; set; }

    public PracticalQuestion PracticalQuestion { get; set; }
}
