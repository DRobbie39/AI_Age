using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class UserPostComment
{
    public int CommentId { get; set; }

    public int PostId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CommentDate { get; set; }

    public bool? Status { get; set; }

    public virtual UserPost Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
