using System;
using System.Collections.Generic;

namespace AI_Age_BackEnd.Models;

public partial class ChatHistory
{
    public int ChatId { get; set; }

    public int UserId { get; set; }

    public string Question { get; set; } = null!;

    public string Response { get; set; } = null!;

    public DateTime? ChatDate { get; set; }

    public virtual User User { get; set; } = null!;
}
