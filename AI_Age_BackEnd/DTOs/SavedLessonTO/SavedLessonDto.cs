namespace AI_Age_BackEnd.DTOs.SavedLessonTO
{
    public class SavedLessonDto
    {
        public int SavedLessonId { get; set; }
        public string LessonType { get; set; }
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string? LessonImage { get; set; }
        public string LessonUrl { get; set; }
        public DateTime SavedDate { get; set; }
    }
}
