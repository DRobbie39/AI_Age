using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class VideoArticle
{
    public int VideoId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string VideoUrl { get; set; } = null!;

    public string? Thumbnail { get; set; }

    public int? Duration { get; set; }

    public int CategoryId { get; set; }

    public int Uploader { get; set; }

    public DateTime? PostedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Views { get; set; }

    public int? Level { get; set; }

    public virtual VideoArticleCategory Category { get; set; } = null!;

    public virtual Admin UploaderNavigation { get; set; } = null!;

    public virtual ICollection<VideoArticleComment> VideoArticleComments { get; set; } = new List<VideoArticleComment>();
}
