using System;
using System.Collections.Generic;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessObjects;

public partial class CosmeticsDbContext : DbContext
{
    public CosmeticsDbContext()
    {
    }

    public CosmeticsDbContext(DbContextOptions<CosmeticsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CosmeticCategory> CosmeticCategories { get; set; }

    public virtual DbSet<CosmeticInformation> CosmeticInformations { get; set; }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CosmeticCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Cosmetic__19093A2BB00EFA6E");

            entity.ToTable("CosmeticCategory");

            entity.Property(e => e.CategoryId)
                .HasMaxLength(30)
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(120);
            entity.Property(e => e.FormulationType).HasMaxLength(250);
            entity.Property(e => e.UsagePurpose).HasMaxLength(250);
        });

        modelBuilder.Entity<CosmeticInformation>(entity =>
        {
            entity.HasKey(e => e.CosmeticId).HasName("PK__Cosmetic__98ED527EC8311EB9");

            entity.ToTable("CosmeticInformation");

            entity.Property(e => e.CosmeticId)
                .HasMaxLength(30)
                .HasColumnName("CosmeticID");
            entity.Property(e => e.CategoryId)
                .HasMaxLength(30)
                .HasColumnName("CategoryID");
            entity.Property(e => e.CosmeticName).HasMaxLength(160);
            entity.Property(e => e.CosmeticSize).HasMaxLength(400);
            entity.Property(e => e.DollarPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ExpirationDate).HasMaxLength(160);
            entity.Property(e => e.SkinType).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.CosmeticInformations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__CosmeticI__Categ__3C69FB99");
        });

        modelBuilder.Entity<SystemAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__SystemAc__349DA586B1D74A03");

            entity.ToTable("SystemAccount");

            entity.HasIndex(e => e.EmailAddress, "UQ__SystemAc__49A14740D606F5BB").IsUnique();

            entity.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("AccountID");
            entity.Property(e => e.AccountNote).HasMaxLength(240);
            entity.Property(e => e.AccountPassword).HasMaxLength(100);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
