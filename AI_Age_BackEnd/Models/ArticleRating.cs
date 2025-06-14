using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class ArticleRating
{
    public int RatingId { get; set; }

    public int ArticleId { get; set; }

    public int? UserId { get; set; }

    public int RatingValue { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual User? User { get; set; }
}
