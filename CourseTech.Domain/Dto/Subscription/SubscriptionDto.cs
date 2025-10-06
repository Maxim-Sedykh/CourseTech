using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Subscription
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxQuestionsPerDay { get; set; }
        public decimal Price { get; set; } // Добавил цену
        public string Description { get; set; } // Добавил описание
        public List<string> Features { get; set; } // Добавил список фич
    }
}
