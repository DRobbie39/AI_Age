using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class VideoArticleRating
{
    public int RatingId { get; set; }

    public int VideoId { get; set; }

    public int? UserId { get; set; }

    public int RatingValue { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? User { get; set; }

    public virtual VideoArticle Video { get; set; } = null!;
}
