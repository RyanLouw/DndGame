using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? Email { get; set; }

    public string? DisplayName { get; set; }

    public string? PhotoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Character? Character { get; set; }
}
