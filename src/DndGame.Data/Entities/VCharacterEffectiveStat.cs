using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class VCharacterEffectiveStat
{
    public Guid CharacterId { get; set; }

    public int? Strength { get; set; }

    public int? Dexterity { get; set; }

    public int? Constitution { get; set; }

    public int? Intelligence { get; set; }

    public int? Wisdom { get; set; }

    public int? Charisma { get; set; }

    public int? MaxHpeffective { get; set; }

    public short CurrentHp { get; set; }

    public short TempHp { get; set; }
}
