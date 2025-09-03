using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndGame.Data.Contracts;

public class CharacterDtos
{

    public class CreateCharacterDto
    {
        [Required, StringLength(128)]
        public string UserId { get; set; } = default!;

        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        [Required, StringLength(50)]
        public string Race { get; set; } = default!;

        [Required, StringLength(50)]
        public string Alignment { get; set; } = default!;

        // Ability scores
        [Range(1, 30)] public int Str { get; set; }
        [Range(1, 30)] public int Dex { get; set; }
        [Range(1, 30)] public int Con { get; set; }
        [Range(1, 30)] public int Int { get; set; }
        [Range(1, 30)] public int Wis { get; set; }
        [Range(1, 30)] public int Cha { get; set; }

        // HP fields
        public int MaxHPBase { get; set; }
        public int CurrentHP { get; set; }
        public int TempHP { get; set; }
    }

    // Used when updating only basics (name, race, alignment)
    public class UpdateBasicsDto
    {
        [Required] public Guid CharacterId { get; set; }
        public string? Name { get; set; }
        public string? Race { get; set; }
        public string? Alignment { get; set; }
        public bool? IsActive { get; set; }
    }

    // Used when updating ability scores / HP
    public class UpdateAbilityScoresDto
    {
        [Required] public Guid CharacterId { get; set; }
        public int? Str { get; set; }
        public int? Dex { get; set; }
        public int? Con { get; set; }
        public int? Int { get; set; }
        public int? Wis { get; set; }
        public int? Cha { get; set; }
        public int? MaxHPBase { get; set; }
        public int? CurrentHP { get; set; }
        public int? TempHP { get; set; }
    }

    // Response when creating or fetching character
    public class CharacterResponseDto
    {
        public Guid CharacterId { get; set; }
        public string Name { get; set; } = default!;
        public string Race { get; set; } = default!;
        public string Alignment { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
