using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Summary { get; set; }

    public string Content { get; set; } = null!;

    public string? Image { get; set; }

    public int CategoryId { get; set; }

    public int Author { get; set; }

    public int? ToolId { get; set; }

    public DateTime? PostedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Views { get; set; }

    public int? Level { get; set; }

    public decimal? AverageRating { get; set; }

    public virtual ICollection<ArticleComment> ArticleComments { get; set; } = new List<ArticleComment>();

    public virtual ICollection<ArticleRating> ArticleRatings { get; set; } = new List<ArticleRating>();

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual ArticleCategory Category { get; set; } = null!;

    public virtual Aitool? Tool { get; set; }
}
