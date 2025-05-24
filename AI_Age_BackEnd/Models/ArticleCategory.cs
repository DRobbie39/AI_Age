using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class ArticleCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public bool? Status { get; set; }

    public virtual Admin? CreatedByNavigation { get; set; }

    public virtual ICollection<TutorialArticle> TutorialArticles { get; set; } = new List<TutorialArticle>();
}
