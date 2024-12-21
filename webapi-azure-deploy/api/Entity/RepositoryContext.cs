using System;
using API.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext : DbContext
{

    public RepositoryContext(DbContextOptions options) : base(options) { }
    public DbSet<Hero> Heros { get; set; }

}