namespace AI_Age_FrontEnd.DTOs
{
    public class VideoArticleDto
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string Thumbnail { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UploaderId { get; set; }
        public string UploaderName { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int Views { get; set; }
        public int Level { get; set; }
        public int Duration { get; set; }
        public decimal? AverageRating { get; set; }
        public int? ToolId { get; set; }
        public string? ToolName { get; set; }
    }
}
