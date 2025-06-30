using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class SavedLesson
{
    public int SavedLessonId { get; set; }

    public int UserId { get; set; }

    public int LessonId { get; set; }

    public string LessonTitle { get; set; } = null!;

    public string? LessonImage { get; set; }

    public DateTime? SavedDate { get; set; }

    public virtual User User { get; set; } = null!;
}
