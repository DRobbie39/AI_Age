using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.VideoArticleDTO
{
    public class VideoArticleCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Video { get; set; }
        public IFormFile Thumbnail { get; set; }
        public int Duration { get; set; }
        public int CategoryId { get; set; }
        public int UploaderId { get; set; }
        public int Level { get; set; }
    }
}
