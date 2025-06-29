using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class ArticleCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
