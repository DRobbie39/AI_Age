﻿using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
