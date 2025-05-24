using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class UserPostCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<UserPost> UserPosts { get; set; } = new List<UserPost>();
}
