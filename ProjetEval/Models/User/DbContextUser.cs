using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjetEval.Models.User
{
    public partial class DbContextUser : DbContext
    {
        public DbContextUser()
        {
        }

        public DbContextUser(DbContextOptions<DbContextUser> options)
            : base(options)
        {
        }

        public virtual DbSet<Profil> Profil { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profil>(entity =>
            {
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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
