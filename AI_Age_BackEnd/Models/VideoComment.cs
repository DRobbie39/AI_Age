using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class VideoComment
{
    public int CommentId { get; set; }

    public int VideoId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CommentDate { get; set; }

    public bool? Status { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual TutorialVideo Video { get; set; } = null!;
}
