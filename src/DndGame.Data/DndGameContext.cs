using System;
using System.Collections.Generic;
using DndGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DndGame.Data;

public partial class DndGameContext : DbContext
{
    public DndGameContext()
    {
    }

    public DndGameContext(DbContextOptions<DndGameContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AbilityScore> AbilityScores { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterEffect> CharacterEffects { get; set; }

    public virtual DbSet<CharacterEffectModifier> CharacterEffectModifiers { get; set; }

    public virtual DbSet<CharacterEquipment> CharacterEquipments { get; set; }

    public virtual DbSet<CharacterQuest> CharacterQuests { get; set; }

    public virtual DbSet<CharacterState> CharacterStates { get; set; }

    public virtual DbSet<EquipmentSlot> EquipmentSlots { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemStatBonuse> ItemStatBonuses { get; set; }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<Quest> Quests { get; set; }

    public virtual DbSet<Spell> Spells { get; set; }

    public virtual DbSet<SpellSlot> SpellSlots { get; set; }

    public virtual DbSet<StoryEvent> StoryEvents { get; set; }

    public virtual DbSet<StoryFlag> StoryFlags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VCharacterEffectiveStat> VCharacterEffectiveStats { get; set; }

    public virtual DbSet<VCurrentCharacterSnapshot> VCurrentCharacterSnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbilityScore>(entity =>
        {
            entity.HasKey(e => e.CharacterId);

            entity.Property(e => e.CharacterId).ValueGeneratedNever();
            entity.Property(e => e.CurrentHp).HasColumnName("CurrentHP");
            entity.Property(e => e.MaxHpbase).HasColumnName("MaxHPBase");
            entity.Property(e => e.TempHp).HasColumnName("TempHP");

            entity.HasOne(d => d.Character).WithOne(p => p.AbilityScore)
                .HasForeignKey<AbilityScore>(d => d.CharacterId)
                .HasConstraintName("FK_AbilityScores_Characters");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.CharacterId).HasName("PK__Characte__757BC9A08A600F52");

            entity.HasIndex(e => e.UserId, "IX_Characters_UserId");

            entity.HasIndex(e => e.UserId, "UX_Characters_User_OneActive")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.CharacterId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Alignment).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Race).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(128);

            entity.HasOne(d => d.User).WithOne(p => p.Character)
                .HasForeignKey<Character>(d => d.UserId)
                .HasConstraintName("FK_Characters_Users");

            entity.HasMany(d => d.Spells).WithMany(p => p.Characters)
                .UsingEntity<Dictionary<string, object>>(
                    "CharacterSpellsKnown",
                    r => r.HasOne<Spell>().WithMany()
                        .HasForeignKey("SpellId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CharacterSpellsKnown_Spells"),
                    l => l.HasOne<Character>().WithMany()
                        .HasForeignKey("CharacterId")
                        .HasConstraintName("FK_CharacterSpellsKnown_Characters"),
                    j =>
                    {
                        j.HasKey("CharacterId", "SpellId");
                        j.ToTable("CharacterSpellsKnown");
                    });

            entity.HasMany(d => d.SpellsNavigation).WithMany(p => p.CharactersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "CharacterSpellsPrepared",
                    r => r.HasOne<Spell>().WithMany()
                        .HasForeignKey("SpellId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CharacterSpellsPrepared_Spells"),
                    l => l.HasOne<Character>().WithMany()
                        .HasForeignKey("CharacterId")
                        .HasConstraintName("FK_CharacterSpellsPrepared_Characters"),
                    j =>
                    {
                        j.HasKey("CharacterId", "SpellId");
                        j.ToTable("CharacterSpellsPrepared");
                    });
        });

        modelBuilder.Entity<CharacterEffect>(entity =>
        {
            entity.HasKey(e => e.EffectId).HasName("PK__Characte__6B859F2389A3F343");

            entity.HasIndex(e => e.CharacterId, "IX_Effects_CharacterId_Active");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Source).HasMaxLength(80);
            entity.Property(e => e.StackingKey).HasMaxLength(50);
            entity.Property(e => e.StartedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterEffects)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_CharacterEffects_Characters");
        });

        modelBuilder.Entity<CharacterEffectModifier>(entity =>
        {
            entity.HasKey(e => new { e.EffectId, e.StatName });

            entity.Property(e => e.StatName).HasMaxLength(20);

            entity.HasOne(d => d.Effect).WithMany(p => p.CharacterEffectModifiers)
                .HasForeignKey(d => d.EffectId)
                .HasConstraintName("FK_CharacterEffectModifiers_Effects");
        });

        modelBuilder.Entity<CharacterEquipment>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.SlotId });

            entity.ToTable("CharacterEquipment");

            entity.HasIndex(e => e.CharacterId, "IX_CharacterEquipment_CharacterId");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterEquipments)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_CharacterEquipment_Characters");

            entity.HasOne(d => d.Item).WithMany(p => p.CharacterEquipments)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterEquipment_Items");

            entity.HasOne(d => d.Slot).WithMany(p => p.CharacterEquipments)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterEquipment_Slots");
        });

        modelBuilder.Entity<CharacterQuest>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.QuestId });

            entity.HasIndex(e => new { e.CharacterId, e.Status }, "IX_CharacterQuests_CharacterId_Status");

            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterQuests)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_CharacterQuests_Characters");

            entity.HasOne(d => d.Quest).WithMany(p => p.CharacterQuests)
                .HasForeignKey(d => d.QuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterQuests_Quests");
        });

        modelBuilder.Entity<CharacterState>(entity =>
        {
            entity.HasKey(e => e.CharacterId);

            entity.ToTable("CharacterState");

            entity.Property(e => e.CharacterId).ValueGeneratedNever();
            entity.Property(e => e.LastCheckpoint).HasMaxLength(200);
            entity.Property(e => e.MapId).HasMaxLength(100);
            entity.Property(e => e.PosX).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.PosY).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.ActiveQuest).WithMany(p => p.CharacterStates)
                .HasForeignKey(d => d.ActiveQuestId)
                .HasConstraintName("FK_CharacterState_Quests");

            entity.HasOne(d => d.Character).WithOne(p => p.CharacterState)
                .HasForeignKey<CharacterState>(d => d.CharacterId)
                .HasConstraintName("FK_CharacterState_Characters");
        });

        modelBuilder.Entity<EquipmentSlot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Equipmen__0A124AAF2F761FEF");

            entity.HasIndex(e => e.Name, "UQ__Equipmen__737584F6500314CD").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(40);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.ItemId });

            entity.HasIndex(e => e.CharacterId, "IX_Inventories_CharacterId");

            entity.HasOne(d => d.Character).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_Inventories_Characters");

            entity.HasOne(d => d.Item).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventories_Items");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Items__727E838B92F9ACD5");

            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.Type).HasMaxLength(40);
            entity.Property(e => e.ValueGp)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ValueGP");
            entity.Property(e => e.Weight).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<ItemStatBonuse>(entity =>
        {
            entity.HasKey(e => new { e.ItemId, e.StatName });

            entity.Property(e => e.StatName).HasMaxLength(20);

            entity.HasOne(d => d.Item).WithMany(p => p.ItemStatBonuses)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ItemStatBonuses_Items");
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.JournalId).HasName("PK__JournalE__250103E62E8897CB");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Character).WithMany(p => p.JournalEntries)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_JournalEntries_Characters");
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(e => e.QuestId).HasName("PK__Quests__B6619A2BFB989E5A");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Spell>(entity =>
        {
            entity.HasKey(e => e.SpellId).HasName("PK__Spells__52BE41BECA4D2C04");

            entity.HasIndex(e => e.Level, "IX_Spells_Level");

            entity.HasIndex(e => e.Name, "UQ__Spells__737584F61142257B").IsUnique();

            entity.Property(e => e.CastingTime).HasMaxLength(60);
            entity.Property(e => e.Components).HasMaxLength(120);
            entity.Property(e => e.Duration).HasMaxLength(60);
            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.School).HasMaxLength(40);
            entity.Property(e => e.SpellRange).HasMaxLength(60);
        });

        modelBuilder.Entity<SpellSlot>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.SpellLevel });

            entity.HasIndex(e => e.CharacterId, "IX_SpellSlots_CharacterId");

            entity.HasOne(d => d.Character).WithMany(p => p.SpellSlots)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_SpellSlots_Characters");
        });

        modelBuilder.Entity<StoryEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__StoryEve__7944C8108C5F7452");

            entity.HasIndex(e => new { e.CharacterId, e.CreatedAt }, "IX_StoryEvents_CharacterId_CreatedAt").IsDescending(false, true);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.EventType).HasMaxLength(50);

            entity.HasOne(d => d.Character).WithMany(p => p.StoryEvents)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_StoryEvents_Characters");
        });

        modelBuilder.Entity<StoryFlag>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.FlagKey });

            entity.HasIndex(e => e.CharacterId, "IX_StoryFlags_CharacterId");

            entity.Property(e => e.FlagKey).HasMaxLength(100);
            entity.Property(e => e.FlagValue).HasMaxLength(200);

            entity.HasOne(d => d.Character).WithMany(p => p.StoryFlags)
                .HasForeignKey(d => d.CharacterId)
                .HasConstraintName("FK_StoryFlags_Characters");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CA97DD944");

            entity.Property(e => e.UserId).HasMaxLength(128);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.PhotoUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<VCharacterEffectiveStat>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_CharacterEffectiveStats");

            entity.Property(e => e.CurrentHp).HasColumnName("CurrentHP");
            entity.Property(e => e.MaxHpeffective).HasColumnName("MaxHPEffective");
            entity.Property(e => e.TempHp).HasColumnName("TempHP");
        });

        modelBuilder.Entity<VCurrentCharacterSnapshot>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_CurrentCharacterSnapshot");

            entity.Property(e => e.ActiveQuestName).HasMaxLength(100);
            entity.Property(e => e.Alignment).HasMaxLength(50);
            entity.Property(e => e.CharacterName).HasMaxLength(100);
            entity.Property(e => e.CurrentHp).HasColumnName("CurrentHP");
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.LastCheckpoint).HasMaxLength(200);
            entity.Property(e => e.MapId).HasMaxLength(100);
            entity.Property(e => e.MaxHp).HasColumnName("MaxHP");
            entity.Property(e => e.PosX).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.PosY).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Race).HasMaxLength(50);
            entity.Property(e => e.TempHp).HasColumnName("TempHP");
            entity.Property(e => e.UserId).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
