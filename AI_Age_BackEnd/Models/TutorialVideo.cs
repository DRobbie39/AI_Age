using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class TutorialVideo
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

    public bool? Status { get; set; }

    public virtual VideoCategory Category { get; set; } = null!;

    public virtual Admin UploaderNavigation { get; set; } = null!;

    public virtual ICollection<VideoComment> VideoComments { get; set; } = new List<VideoComment>();
}
