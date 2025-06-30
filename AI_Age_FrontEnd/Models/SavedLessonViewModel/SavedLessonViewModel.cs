namespace AI_Age_FrontEnd.Models.SavedLessonViewModel
{
    public class SavedLessonViewModel
    {
        public int SavedLessonId { get; set; }
        public int LessonId { get; set; }
        public string LessonType { get; set; }
        public string LessonTitle { get; set; }
        public string? LessonImage { get; set; }
        public string LessonUrl { get; set; }
        public DateTime SavedDate { get; set; }
    }
}
