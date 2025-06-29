using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class Aitool
{
    public int ToolId { get; set; }

    public string ToolName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? LogoUrl { get; set; }

    public string? WebsiteUrl { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual AitoolCategory? Category { get; set; }

    public virtual ICollection<VideoArticle> VideoArticles { get; set; } = new List<VideoArticle>();
}
