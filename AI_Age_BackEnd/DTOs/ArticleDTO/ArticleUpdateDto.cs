using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.ArticleDTO
{
    public class ArticleUpdateDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
        public int Level { get; set; }
    }
}
