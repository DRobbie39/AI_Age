namespace AI_Age_BackEnd.DTOs.RatingDTO
{
    public class ArticleRatingCreateDto
    {
        public int ArticleId { get; set; }
        public int? UserId { get; set; }
        public int RatingValue { get; set; }
    }
}
