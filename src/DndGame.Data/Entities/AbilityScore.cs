using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class AbilityScore
{
    public Guid CharacterId { get; set; }

    public byte StrengthBase { get; set; }

    public byte DexterityBase { get; set; }

    public byte ConstitutionBase { get; set; }

    public byte IntelligenceBase { get; set; }

    public byte WisdomBase { get; set; }

    public byte CharismaBase { get; set; }

    public short MaxHpbase { get; set; }

    public short CurrentHp { get; set; }

    public short TempHp { get; set; }

    public virtual Character Character { get; set; } = null!;
}
