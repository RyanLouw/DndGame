using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class ClassSavingThrow
{
    public Guid ClassId { get; set; }

    public string Ability { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;
}
