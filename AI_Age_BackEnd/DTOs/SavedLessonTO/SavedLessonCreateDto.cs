namespace AI_Age_BackEnd.DTOs.SavedLessonTO
{
    public class SavedLessonCreateDto
    {
        public int? ArticleId { get; set; }
        public int? VideoId { get; set; }
        public string LessonTitle { get; set; }
        public string? LessonImage { get; set; }
        public string LessonUrl { get; set; }
    }
}
