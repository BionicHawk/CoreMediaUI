using System;
using System.Collections.Generic;
using CoreMediaUI.Source.Config;
using Microsoft.EntityFrameworkCore;

namespace CoreMediaUI.Models;

public partial class ConfigurationContext : DbContext
{
    public ConfigurationContext()
    {
    }

    public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Setting> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={ConfigBuilder.DatabaseFilePath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.SettingsId);

            entity.Property(e => e.SettingsId)
                .ValueGeneratedNever()
                .HasColumnType("INTEGET")
                .HasColumnName("settings_id");
            entity.Property(e => e.SelectedIp).HasColumnName("selected_ip");
            entity.Property(e => e.Sensibility)
                .HasDefaultValue(100.0)
                .HasColumnName("sensibility");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
