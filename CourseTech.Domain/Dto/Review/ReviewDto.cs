namespace CourseTech.Domain.Dto.Review;

/// <summary>
/// Модель данных для отображения отзыва.
/// </summary>
public class ReviewDto
{
    public long Id { get; set; }

    public Guid UserId { get; set; }

    public string Login { get; set; }

    public string ReviewText { get; set; }

    public string CreatedAt { get; set; }
}
