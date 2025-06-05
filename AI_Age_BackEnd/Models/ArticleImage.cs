using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class ArticleImage
{
    public int ImageId { get; set; }

    public int ArticleId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;
}
