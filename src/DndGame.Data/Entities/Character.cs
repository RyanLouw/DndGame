using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Character
{
    public Guid CharacterId { get; set; }

    public string UserId { get; set; } = null!;

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string? Race { get; set; }

    public string? Alignment { get; set; }

    public string? Backstory { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AbilityScore? AbilityScore { get; set; }

    public virtual ICollection<CharacterEffect> CharacterEffects { get; set; } = new List<CharacterEffect>();

    public virtual ICollection<CharacterEquipment> CharacterEquipments { get; set; } = new List<CharacterEquipment>();

    public virtual ICollection<CharacterQuest> CharacterQuests { get; set; } = new List<CharacterQuest>();

    public virtual CharacterState? CharacterState { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();

    public virtual ICollection<SpellSlot> SpellSlots { get; set; } = new List<SpellSlot>();

    public virtual ICollection<StoryEvent> StoryEvents { get; set; } = new List<StoryEvent>();

    public virtual ICollection<StoryFlag> StoryFlags { get; set; } = new List<StoryFlag>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Spell> Spells { get; set; } = new List<Spell>();

    public virtual ICollection<Spell> SpellsNavigation { get; set; } = new List<Spell>();
}
