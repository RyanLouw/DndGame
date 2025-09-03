using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DndGame.Data.Entities;

public partial class DndroleContext : DbContext
{
    public DndroleContext()
    {
    }

    public DndroleContext(DbContextOptions<DndroleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CasterProgression> CasterProgressions { get; set; }

    public virtual DbSet<CharacterClass> CharacterClasses { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassFeature> ClassFeatures { get; set; }

    public virtual DbSet<ClassSavingThrow> ClassSavingThrows { get; set; }

    public virtual DbSet<Proficiency> Proficiencies { get; set; }

    public virtual DbSet<Subclass> Subclasses { get; set; }

    public virtual DbSet<SubclassFeature> SubclassFeatures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DNDROLE;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CasterProgression>(entity =>
        {
            entity.HasKey(e => new { e.CasterType, e.CharacterLevel });

            entity.ToTable("CasterProgression");
        });

        modelBuilder.Entity<CharacterClass>(entity =>
        {
            entity.HasKey(e => new { e.CharacterId, e.ClassId });

            entity.HasIndex(e => e.CharacterId, "IX_CharacterClasses_CharacterId");

            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ClassLevel).HasDefaultValue(1);

            entity.HasOne(d => d.Class).WithMany(p => p.CharacterClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterClasses_Classes");

            entity.HasOne(d => d.Subclass).WithMany(p => p.CharacterClasses)
                .HasForeignKey(d => d.SubclassId)
                .HasConstraintName("FK_CharacterClasses_Subclasses");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ_Classes_Name").IsUnique();

            entity.Property(e => e.ClassId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Name).HasMaxLength(60);
            entity.Property(e => e.PrimaryAbility).HasMaxLength(20);
            entity.Property(e => e.SpellcastingAbility).HasMaxLength(20);

            entity.HasMany(d => d.Proficiencies).WithMany(p => p.Classes)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassProficiency",
                    r => r.HasOne<Proficiency>().WithMany()
                        .HasForeignKey("ProficiencyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ClassProficiencies_Prof"),
                    l => l.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ClassProficiencies_Class"),
                    j =>
                    {
                        j.HasKey("ClassId", "ProficiencyId");
                        j.ToTable("ClassProficiencies");
                    });
        });

        modelBuilder.Entity<ClassFeature>(entity =>
        {
            entity.HasKey(e => e.FeatureId);

            entity.HasIndex(e => new { e.ClassId, e.Level, e.Name }, "IX_ClassFeatures_Class_Level").IsUnique();

            entity.Property(e => e.FeatureId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsPassive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasOne(d => d.Class).WithMany(p => p.ClassFeatures)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassFeatures_Classes");
        });

        modelBuilder.Entity<ClassSavingThrow>(entity =>
        {
            entity.HasKey(e => new { e.ClassId, e.Ability });

            entity.Property(e => e.Ability).HasMaxLength(3);

            entity.HasOne(d => d.Class).WithMany(p => p.ClassSavingThrows)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassSavingThrows_Classes");
        });

        modelBuilder.Entity<Proficiency>(entity =>
        {
            entity.HasIndex(e => new { e.Type, e.Name }, "UQ_Proficiencies").IsUnique();

            entity.Property(e => e.ProficiencyId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<Subclass>(entity =>
        {
            entity.HasIndex(e => new { e.ClassId, e.Name }, "UQ_Subclasses_Class_Name").IsUnique();

            entity.Property(e => e.SubclassId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.LevelAvailable).HasDefaultValue(3);
            entity.Property(e => e.Name).HasMaxLength(80);

            entity.HasOne(d => d.Class).WithMany(p => p.Subclasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subclasses_Classes");
        });

        modelBuilder.Entity<SubclassFeature>(entity =>
        {
            entity.HasKey(e => e.FeatureId);

            entity.HasIndex(e => new { e.SubclassId, e.Level, e.Name }, "IX_SubclassFeatures_Subclass_Level").IsUnique();

            entity.Property(e => e.FeatureId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsPassive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasOne(d => d.Subclass).WithMany(p => p.SubclassFeatures)
                .HasForeignKey(d => d.SubclassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubclassFeatures_Subclasses");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
