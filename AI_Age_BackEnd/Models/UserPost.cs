﻿using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class UserPost
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? AttachedImage { get; set; }

    public int UserId { get; set; }

    public DateTime? PostedDate { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserPostComment> UserPostComments { get; set; } = new List<UserPostComment>();
}
