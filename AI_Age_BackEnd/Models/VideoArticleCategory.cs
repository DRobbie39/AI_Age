using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class VideoArticleCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<VideoArticle> VideoArticles { get; set; } = new List<VideoArticle>();
}
