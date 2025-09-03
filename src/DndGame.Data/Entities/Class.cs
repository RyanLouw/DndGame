using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Class
{
    public Guid ClassId { get; set; }

    public string Name { get; set; } = null!;

    public byte HitDie { get; set; }

    public string? PrimaryAbility { get; set; }

    public byte CasterType { get; set; }

    public string? SpellcastingAbility { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CharacterClass> CharacterClasses { get; set; } = new List<CharacterClass>();

    public virtual ICollection<ClassFeature> ClassFeatures { get; set; } = new List<ClassFeature>();

    public virtual ICollection<ClassSavingThrow> ClassSavingThrows { get; set; } = new List<ClassSavingThrow>();

    public virtual ICollection<Subclass> Subclasses { get; set; } = new List<Subclass>();

    public virtual ICollection<Proficiency> Proficiencies { get; set; } = new List<Proficiency>();
}
