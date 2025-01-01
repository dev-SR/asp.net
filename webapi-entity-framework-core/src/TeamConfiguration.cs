using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyConsoleApp;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {

        builder.HasMany(m => m.HomeMatches)
            .WithOne(m => m.HomeTeam)
            .HasForeignKey(m => m.HomeTeamId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.AwayMatches)
            .WithOne(m => m.AwayTeam)
            .HasForeignKey(m => m.AwayTeamId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
