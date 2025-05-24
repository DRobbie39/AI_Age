using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TutorialArticle> TutorialArticles { get; set; } = new List<TutorialArticle>();

    public virtual ICollection<TutorialVideo> TutorialVideos { get; set; } = new List<TutorialVideo>();

    public virtual ICollection<VideoCategory> VideoCategories { get; set; } = new List<VideoCategory>();
}
