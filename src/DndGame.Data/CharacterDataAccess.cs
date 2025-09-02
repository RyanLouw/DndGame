using DndGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndGame.Data
{
    public class CharacterDataAccess
    {

        private readonly DndGameContext _db;
        public CharacterDataAccess(DndGameContext db) => _db = db;

        // ---------------------------
        // CREATE
        // ---------------------------
        /// <summary>
        /// Creates a character and its AbilityScores + CharacterState atomically.
        /// Enforces one-active-per-user by deactivating any existing actives.
        /// </summary>
        public async Task<Character> CreateCharacterAsync(
            string userId,
            string name,
            string race,
            string alignment,
            int str, int dex, int con, int intel, int wis, int cha,
            int maxHpBase, int currentHp, int tempHp,
            string? mapId = "start_area", decimal posX = 0, decimal posY = 0, string? lastCheckpoint = "start",
            CancellationToken ct = default)
        {
            using var tx = await _db.Database.BeginTransactionAsync(ct);

            // Deactivate existing active character(s) for this user
            var actives = await _db.Characters
                .Where(c => c.UserId == userId && c.IsActive)
                .ToListAsync(ct);
            foreach (var a in actives) a.IsActive = false;
            if (actives.Count > 0) await _db.SaveChangesAsync(ct);

            var character = new Character
            {
                CharacterId = Guid.NewGuid(), // we set it to link child rows now
                UserId = userId.Trim(),
                Name = name.Trim(),
                Race = race.Trim(),
                Alignment = alignment.Trim(),
                IsActive = true
            };
            _db.Characters.Add(character);

            _db.AbilityScores.Add(new AbilityScore
            {
                CharacterId = character.CharacterId,
                StrengthBase = (byte)str,
                DexterityBase = (byte)dex,
                ConstitutionBase = (byte)con,
                IntelligenceBase = (byte)intel,
                WisdomBase = (byte)wis,
                CharismaBase = (byte)cha,
                MaxHpbase = (byte)maxHpBase,
                CurrentHp = (byte)currentHp,
                TempHp = (byte)tempHp
            });

            _db.CharacterStates.Add(new CharacterState
            {
                CharacterId = character.CharacterId,
                MapId = mapId,
                PosX = posX,
                PosY = posY,
                LastCheckpoint = lastCheckpoint
            });

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return character;
        }

        // ---------------------------
        // BASICS
        // ---------------------------
        public async Task<Character> UpdateBasicsAsync(
            Guid characterId,
            string? name = null, string? race = null, string? alignment = null,
            bool? isActive = null,
            CancellationToken ct = default)
        {
            var ch = await _db.Characters.FirstOrDefaultAsync(c => c.CharacterId == characterId, ct)
                     ?? throw new InvalidOperationException("Character not found.");

            if (!string.IsNullOrWhiteSpace(name)) ch.Name = name.Trim();
            if (!string.IsNullOrWhiteSpace(race)) ch.Race = race.Trim();
            if (!string.IsNullOrWhiteSpace(alignment)) ch.Alignment = alignment.Trim();

            if (isActive.HasValue)
            {
                if (isActive.Value)
                {
                    // Activate this one and deactivate others for same user
                    var others = await _db.Characters
                        .Where(c => c.UserId == ch.UserId && c.CharacterId != ch.CharacterId && c.IsActive)
                        .ToListAsync(ct);
                    foreach (var o in others) o.IsActive = false;
                    ch.IsActive = true;
                }
                else
                {
                    ch.IsActive = false;
                }
            }

            await _db.SaveChangesAsync(ct);
            return ch;
        }

        // ---------------------------
        // ABILITY SCORES / HP
        // ---------------------------
        public async Task<AbilityScore> UpdateAbilityScoresAsync(
            Guid characterId,
            int? str = null, int? dex = null, int? con = null,
            int? intel = null, int? wis = null, int? cha = null,
            int? maxHpBase = null, int? currentHp = null, int? tempHp = null,
            CancellationToken ct = default)
        {
            var ab = await _db.AbilityScores.FirstOrDefaultAsync(a => a.CharacterId == characterId, ct)
                     ?? throw new InvalidOperationException("AbilityScores row not found.");

            if (str.HasValue) ab.StrengthBase = (byte)str.Value;
            if (dex.HasValue) ab.DexterityBase = (byte)dex.Value;
            if (con.HasValue) ab.ConstitutionBase = (byte)con.Value;
            if (intel.HasValue) ab.IntelligenceBase = (byte)intel.Value;
            if (wis.HasValue) ab.WisdomBase = (byte)wis.Value;
            if (cha.HasValue) ab.CharismaBase = (byte)cha.Value;

            if (maxHpBase.HasValue) ab.MaxHpbase = (short)maxHpBase.Value;
            if (currentHp.HasValue) ab.CurrentHp = (short)currentHp.Value;
            if (tempHp.HasValue) ab.TempHp = (short)tempHp.Value;

            await _db.SaveChangesAsync(ct);
            return ab;
        }


        // ---------------------------
        // INVENTORY (add/remove quantities)
        // ---------------------------
        /// <summary>
        /// Apply multiple inventory quantity deltas (add/remove). Net &lt;= 0 removes the row.
        /// </summary>
        public async Task SetInventoryAsync(
            Guid characterId,
            IEnumerable<(int ItemId, int QuantityDelta)> changes,
            CancellationToken ct = default)
        {
            // Ensure character exists
            var exists = await _db.Characters.AnyAsync(c => c.CharacterId == characterId, ct);
            if (!exists) throw new InvalidOperationException("Character not found.");

            foreach (var (itemId, delta) in changes)
            {
                var row = await _db.Inventories
                    .FirstOrDefaultAsync(i => i.CharacterId == characterId && i.ItemId == itemId, ct);

                if (row == null)
                {
                    if (delta > 0)
                    {
                        _db.Inventories.Add(new Inventory
                        {
                            CharacterId = characterId,
                            ItemId = itemId,
                            Quantity = delta
                        });
                    }
                }
                else
                {
                    row.Quantity += delta;
                    if (row.Quantity <= 0)
                        _db.Inventories.Remove(row);
                }
            }

            await _db.SaveChangesAsync(ct);
        }

        // ---------------------------
        // EQUIPMENT (per slot)
        // ---------------------------
        /// <summary>
        /// Equip an item into a slot (or pass null itemId to unequip).
        /// </summary>
        public async Task SetEquipmentAsync(Guid characterId, int slotId, int? itemId, CancellationToken ct = default)
        {
            var row = await _db.CharacterEquipments
                .FirstOrDefaultAsync(e => e.CharacterId == characterId && e.SlotId == slotId, ct);

            if (itemId is null)
            {
                if (row != null) _db.CharacterEquipments.Remove(row);
            }
            else
            {
                if (row == null)
                {
                    _db.CharacterEquipments.Add(new CharacterEquipment
                    {
                        CharacterId = characterId,
                        SlotId = slotId,
                        ItemId = itemId.Value
                    });
                }
                else
                {
                    row.ItemId = itemId.Value;
                }
            }

            await _db.SaveChangesAsync(ct);
        }

        // ---------------------------
        // SPELLS (Known / Prepared) – overwrite full sets
        // ---------------------------
        public async Task SetKnownSpellsAsync(Guid characterId, IEnumerable<int> spellIds, CancellationToken ct = default)
        {
            var table = _db.Set<Dictionary<string, object>>("CharacterSpellsKnown");

            var existing = await table
                .Where(e => (Guid)e["CharacterId"] == characterId)
                .ToListAsync(ct);

            if (existing.Count > 0)
            {
                _db.RemoveRange(existing);
                await _db.SaveChangesAsync(ct);
            }

            foreach (var sid in spellIds.Distinct())
            {
                table.Add(new Dictionary<string, object?>
                {
                    ["CharacterId"] = characterId,
                    ["SpellId"] = sid
                });
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task SetPreparedSpellsAsync(Guid characterId, IEnumerable<int> spellIds, CancellationToken ct = default)
        {
            var table = _db.Set<Dictionary<string, object>>("CharacterSpellsPrepared");

            var existing = await table
                .Where(e => (Guid)e["CharacterId"] == characterId)
                .ToListAsync(ct);

            if (existing.Count > 0)
            {
                _db.RemoveRange(existing);
                await _db.SaveChangesAsync(ct);
            }

            foreach (var sid in spellIds.Distinct())
            {
                table.Add(new Dictionary<string, object?>
                {
                    ["CharacterId"] = characterId,
                    ["SpellId"] = sid
                });
            }

            await _db.SaveChangesAsync(ct);
        }

        // ---------------------------
        // SPELL SLOTS (upsert per level)
        // ---------------------------
        public async Task SetSpellSlotsAsync(
     Guid characterId,
     IEnumerable<(int SpellLevel, int SlotsMax, int SlotsCur)> perLevel,
     CancellationToken ct = default)
        {
            foreach (var (lvl, max, cur) in perLevel)
            {
                var row = await _db.SpellSlots
                    .FirstOrDefaultAsync(s => s.CharacterId == characterId && s.SpellLevel == (byte)lvl, ct);

                if (row == null)
                {
                    _db.SpellSlots.Add(new SpellSlot
                    {
                        CharacterId = characterId,
                        SpellLevel = (byte)lvl,
                        SlotsMax = (byte)max,
                        SlotsCur = (byte)cur
                    });
                }
                else
                {
                    row.SlotsMax = (byte)max;
                    row.SlotsCur = (byte)cur;
                }
            }

            await _db.SaveChangesAsync(ct);
        }


        // ---------------------------
        // READ HELPERS (views)
        // ---------------------------
        public Task<VCurrentCharacterSnapshot?> GetCurrentSnapshotAsync(string userId, CancellationToken ct = default) =>
            _db.VCurrentCharacterSnapshots.FirstOrDefaultAsync(v => v.UserId == userId, ct);

        public Task<List<VCharacterEffectiveStat>> GetEffectiveStatsAsync(Guid characterId, CancellationToken ct = default) =>
            _db.VCharacterEffectiveStats.Where(v => v.CharacterId == characterId).ToListAsync(ct);
    }
}
