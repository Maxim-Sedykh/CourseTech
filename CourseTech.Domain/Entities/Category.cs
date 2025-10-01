using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities
{
    public class Category : IAuditable, IEntityId<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание категории. Общая информация про неё.
        /// </summary>
        public string Description { get; set; }

        public string IconUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
