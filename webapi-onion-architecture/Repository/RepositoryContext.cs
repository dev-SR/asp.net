using System;
using Bogus;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repository.Configuration;
using Repository.Seeding;

namespace Repository;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed the data
        SeedData.SeedCompanies(10);  // Generate 10 companies
        SeedData.SeedEmployees(50); // Generate 50 employees

        // Apply the seed data
        modelBuilder.Entity<Company>().HasData(SeedData.Companies);
        modelBuilder.Entity<Employee>().HasData(SeedData.Employees);
    }
    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}