using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public int RoleId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ArticleComment> ArticleComments { get; set; } = new List<ArticleComment>();

    public virtual ICollection<ArticleRating> ArticleRatings { get; set; } = new List<ArticleRating>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<UserPostComment> UserPostComments { get; set; } = new List<UserPostComment>();

    public virtual ICollection<UserPost> UserPosts { get; set; } = new List<UserPost>();

    public virtual ICollection<VideoArticleComment> VideoArticleComments { get; set; } = new List<VideoArticleComment>();
}
