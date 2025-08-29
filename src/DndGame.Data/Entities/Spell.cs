using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Spell
{
    public int SpellId { get; set; }

    public string Name { get; set; } = null!;

    public byte Level { get; set; }

    public string? School { get; set; }

    public string? CastingTime { get; set; }

    public string? SpellRange { get; set; }

    public string? Components { get; set; }

    public string? Duration { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<Character> CharactersNavigation { get; set; } = new List<Character>();
}
