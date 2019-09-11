using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PolovniAutomobiliZavrsniRad.Models
{
    public partial class AutoContext : DbContext
    {
        public AutoContext()
        {
        }

        public AutoContext(DbContextOptions<AutoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Komentar> Kometari { get; set; }
        public virtual DbSet<Marka> Marke { get; set; }
        public virtual DbSet<Modeli> Models { get; set; }
        public virtual DbSet<TipVozila> TipoviVozila { get; set; }
        public virtual DbSet<Vozilo> Vozila { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Adresa).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Grad).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Ime).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Prezime).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Telefon).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<Komentar>(entity =>
            {
                entity.HasOne(d => d.Vozilo)
                    .WithMany(p => p.Komentars)
                    .HasForeignKey(d => d.VoziloId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Komentar__Vozilo__29221CFB");
            });

            modelBuilder.Entity<Modeli>(entity =>
            {
                entity.HasKey(e => e.ModelId)
                    .HasName("PK__Modeli__E8D7A12CCDD27570");

                entity.HasOne(d => d.Marka)
                    .WithMany(p => p.Modelis)
                    .HasForeignKey(d => d.MarkaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Modeli__MarkaId__4BAC3F29");
            });

            modelBuilder.Entity<Vozilo>(entity =>
            {
                entity.Property(e => e.BrojBrzina).IsUnicode(false);

                entity.Property(e => e.Kilometraza).IsUnicode(false);

                entity.HasOne(d => d.Marka)
                    .WithMany(p => p.Vozilos)
                    .HasForeignKey(d => d.MarkaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vozilo__MarkaId__5070F446");

                entity.HasOne(d => d.Modeli)
                    .WithMany(p => p.Vozilos)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vozilo__ModelId__5165187F");

                entity.HasOne(d => d.TipVozila)
                    .WithMany(p => p.Vozilos)
                    .HasForeignKey(d => d.TipVozilaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vozilo__TipVozil__52593CB8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}