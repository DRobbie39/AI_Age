﻿using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class AitoolCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Aitool> Aitools { get; set; } = new List<Aitool>();
}
