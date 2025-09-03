using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class CasterProgression
{
    public byte CasterType { get; set; }

    public int CharacterLevel { get; set; }

    public byte? Slots1 { get; set; }

    public byte? Slots2 { get; set; }

    public byte? Slots3 { get; set; }

    public byte? Slots4 { get; set; }

    public byte? Slots5 { get; set; }

    public byte? Slots6 { get; set; }

    public byte? Slots7 { get; set; }

    public byte? Slots8 { get; set; }

    public byte? Slots9 { get; set; }

    public byte? PactSlots { get; set; }

    public byte? PactSlotLevel { get; set; }
}
