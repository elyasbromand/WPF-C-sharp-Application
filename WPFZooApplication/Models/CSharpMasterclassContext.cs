using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WPFZooApplication.Models;

public partial class CSharpMasterclassContext : DbContext
{
    public CSharpMasterclassContext() { }

    public CSharpMasterclassContext(DbContextOptions<CSharpMasterclassContext> options)
        : base(options) { }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<Zoo> Zoos { get; set; }

    public virtual DbSet<ZooAnimal> ZooAnimals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable(
                "CSharpMasterclassConnectionString"
            );

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string not found. Set the environment variable 'CSharpMasterclassConnectionString'."
                );
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Animal__3214EC07DE387D68");

            entity.ToTable("Animal");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Zoo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Zoo__3214EC07834F17B5");

            entity.ToTable("Zoo");

            entity.Property(e => e.Location).HasMaxLength(50).HasColumnName("location");
        });

        modelBuilder.Entity<ZooAnimal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ZooAnima__3214EC07CDAE8617");

            entity.ToTable("ZooAnimal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
