using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjetEval.Models.TestUser;

public partial class DbContextUserTest : DbContext
{
    public DbContextUserTest()
    {
    }

    public DbContextUserTest(DbContextOptions<DbContextUserTest> options)
        : base(options)
    {
    }

    public virtual DbSet<Genre> Genre { get; set; }

    public virtual DbSet<Profil> Profil { get; set; }

    public virtual DbSet<Utilisateur> Utilisateur { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genre_pkey");

            entity.ToTable("genre");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Genre non initialiser'::character varying")
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Profil>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profil_pkey");

            entity.ToTable("profil");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Idgenre).HasColumnName("idgenre");
            entity.Property(e => e.Max).HasColumnName("max");
            entity.Property(e => e.Min).HasColumnName("min");
            entity.Property(e => e.Naissance)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("naissance");
            entity.Property(e => e.Nom)
                .HasMaxLength(200)
                .HasColumnName("nom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(300)
                .HasColumnName("prenom");

            entity.HasOne(d => d.IdgenreNavigation).WithMany(p => p.Profils)
                .HasForeignKey(d => d.Idgenre)
                .HasConstraintName("profil_idgenre_fkey");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("utilisateur_pkey");

            entity.ToTable("utilisateur");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(500)
                .HasColumnName("adress");
            entity.Property(e => e.Idprofil).HasColumnName("idprofil");
            entity.Property(e => e.Mdp)
                .HasMaxLength(100)
                .HasColumnName("mdp");
            entity.Property(e => e.Recuperation)
                .HasMaxLength(500)
                .HasColumnName("recuperation");

            entity.HasOne(d => d.IdprofilNavigation).WithMany(p => p.Utilisateurs)
                .HasForeignKey(d => d.Idprofil)
                .HasConstraintName("utilisateur_idprofil_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
