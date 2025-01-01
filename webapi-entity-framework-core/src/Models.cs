using System;

namespace MyConsoleApp;



public class League
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // One-to-many relationship: A League can have many Teams
    public virtual List<Team> Teams { get; set; } = new List<Team>();//or assign `null!` null-forgiving operator
}

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // Foreign key for the League
    public int LeagueId { get; set; }
    // Navigation property for the League (many-to-one)
    public virtual League League { get; set; } = null!;
    // Navigation property for the one-to-one relationship with Coach
    public virtual Coach? Coach { get; set; }
    // Navigation properties for matches
    public virtual List<Match> HomeMatches { get; set; } = new List<Match>();
    public virtual List<Match> AwayMatches { get; set; } = new List<Match>();
}

/* 

TeamA -> Match (HomeTeam, AwayTeam) <- TeamB

TeamA has many HomeMatches (1-M)
TeamB has many AwayMatches (1-M)
Teams has many Matches with other Teams (M-M)

Similar to :

Customer -> Order(CustomerId, ProductId) <- Product

Customer makes M Orders (1-M)
Product has M Orders (M-1)
Many products has many Cu
 */
public class Match
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public virtual Team HomeTeam { get; set; } = null!;
    public int AwayTeamId { get; set; }
    public virtual Team AwayTeam { get; set; } = null!;
    public decimal TicketPrice { get; set; }
    public DateTime Date { get; set; }
}

/* 
This type of complex relationship must be handled with fluent API
> Without fluent API, migration will throw the following error >  The exception 'Unable to determine the relationship represented by navigation 'Match.AwayTeam' of type 'Team'.
 
Therefore see > TeamConfiguration class for the configuration of the relationship between Team and Match

Other relationships are following the convention and do not require fluent API
 */
public class Coach
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // Nullable foreign key for the Team
    public int? TeamId { get; set; }
    // Navigation property for the one-to-one relationship with Team
    public virtual Team? Team { get; set; }
}

