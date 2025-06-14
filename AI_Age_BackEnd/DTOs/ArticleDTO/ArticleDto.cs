namespace AI_Age_BackEnd.DTOs.ArticleDTO
{
    public class ArticleDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Views { get; set; }
        public int? Level { get; set; }
        public decimal AverageRating { get; set; }
    }
}
