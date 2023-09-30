﻿
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ServiceModule.Infrastructure;
public class ServiceDbContext : DbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 2);
        configurationBuilder
            .Properties<decimal?>()
            .HavePrecision(18, 2);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if RELEASE
#else
        optionsBuilder.EnableDetailedErrors();
#endif
    }
}