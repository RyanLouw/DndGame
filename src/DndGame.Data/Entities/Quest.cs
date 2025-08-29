using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Quest
{
    public int QuestId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CharacterQuest> CharacterQuests { get; set; } = new List<CharacterQuest>();

    public virtual ICollection<CharacterState> CharacterStates { get; set; } = new List<CharacterState>();
}
