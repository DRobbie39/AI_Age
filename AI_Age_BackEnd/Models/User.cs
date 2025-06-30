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

    public virtual ICollection<ArticleRating> ArticleRatings { get; set; } = new List<ArticleRating>();

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<ChatHistory> ChatHistories { get; set; } = new List<ChatHistory>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SavedLesson> SavedLessons { get; set; } = new List<SavedLesson>();

    public virtual ICollection<UserPostComment> UserPostComments { get; set; } = new List<UserPostComment>();

    public virtual ICollection<UserPost> UserPosts { get; set; } = new List<UserPost>();

    public virtual ICollection<VideoArticleRating> VideoArticleRatings { get; set; } = new List<VideoArticleRating>();

    public virtual ICollection<VideoArticle> VideoArticles { get; set; } = new List<VideoArticle>();
}
