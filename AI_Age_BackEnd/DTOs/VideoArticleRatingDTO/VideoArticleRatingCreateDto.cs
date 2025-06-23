namespace AI_Age_BackEnd.DTOs.VideoArticleRatingDTO
{
    public class VideoArticleRatingCreateDto
    {
        public int VideoId { get; set; }
        public int? UserId { get; set; }
        public int RatingValue { get; set; }
    }
}
