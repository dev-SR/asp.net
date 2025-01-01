// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using MyConsoleApp;

Console.WriteLine("Hello, World!");
RepositoryDBContext context = new RepositoryDBContext();
// context.Database.Migrate();
// clear tables
await context.Matches.ExecuteDeleteAsync();

await context.Leagues.ExecuteDeleteAsync();
await context.Teams.ExecuteDeleteAsync();

// inseer
async Task<int> AddSingleRecord()
{
    var league = new League
    {
        Name = "League 1",
        // Id = not required
    };
    await context.Leagues.AddAsync(league);
    await context.SaveChangesAsync();

    return league.Id;
}
var league1Id = await AddSingleRecord();
async Task<List<League>> AddBulkRecord()
{
    var leagues = new List<League> {
        new League{Name = "League 2"},
        new League{Name = "League 3"},
    };

    await context.Leagues.AddRangeAsync(leagues);
    await context.SaveChangesAsync();

    return leagues;

}
var leagues = await AddBulkRecord();


async Task create_dependent_and_connect_principal_by_fk()
{
    /* 
        var teamChild = new Team
        {
            Name = "Team 1"
        }; 
        
        will throw exception > 'FOREIGN KEY constraint failed'.
        
        Must connect with principal entity through foreign key or navigation property
    */

    var leagueParent = await context.Leagues.FirstOrDefaultAsync(l => l.Id == league1Id);
    if (leagueParent != null)
    {
        var teamChild = new Team
        {
            Name = "Team 1",
            LeagueId = leagueParent.Id
        };
        await context.Teams.AddAsync(teamChild);
        await context.SaveChangesAsync();
    }
}
await create_dependent_and_connect_principal_by_fk();

async Task create_dependent_and_connect_principal_by_navigation_property()
{
    var leagueParent = await context.Leagues.FindAsync(league1Id);
    if (leagueParent != null)
    {
        var teamChild = new Team
        {
            Name = "Team 2",
            League = leagueParent
        };
        await context.Teams.AddAsync(teamChild);
        await context.SaveChangesAsync();

    }
}
await create_dependent_and_connect_principal_by_navigation_property();


async Task create_both_dependent_and_principal_and_connect_them_by_id()
{
    try
    {
        var league = new League { Name = "League 4" };
        await context.Leagues.AddAsync(league);
        await context.SaveChangesAsync();// must be called before adding team; otherwise, league.Id will be null
        Console.WriteLine($"League Id: {league.Id}");
        var team = new Team { Name = "Team 4", LeagueId = league.Id };
        await context.Teams.AddAsync(team);
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
await create_both_dependent_and_principal_and_connect_them_by_id();


async Task create_both_dependent_and_principal_and_connect_them_by_nav_property()
{
    try
    {
        var league = new League { Name = "League 4" };
        var team = new Team { Name = "Team 4", League = league };
        await context.Teams.AddAsync(team);
        await context.SaveChangesAsync();

        /* 
            No need to add league to context
            ⛔await context.Leagues.AddAsync(league);
            ⛔await context.SaveChangesAsync();
            
            SaveChanges will automatically add league to context
         */
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
await create_both_dependent_and_principal_and_connect_them_by_nav_property();


async Task create_principal_and_dependents_and_connect_them()
{
    var teams = new List<Team>
    {
        new Team { Name = "Team 5" },
        new Team { Name = "Team 6" }
    };

    var league = new League { Name = "League 5", Teams = teams };
    await context.Leagues.AddAsync(league);
    await context.SaveChangesAsync();
}
await create_principal_and_dependents_and_connect_them();


async Task create_many_to_many_relationship()
{
    var team1 = new Team { Name = "Team 7", LeagueId = league1Id };
    var team2 = new Team { Name = "Team 8", LeagueId = league1Id };
    var match = new Match { HomeTeam = team1, AwayTeam = team2, TicketPrice = 100, Date = DateTime.Now };
    await context.Matches.AddAsync(match);
    await context.SaveChangesAsync();

    var teams = await context.Teams.ToListAsync();
    var id1 = teams[0].Id;
    var id2 = teams[1].Id;

    var teamA = await context.Teams.FirstOrDefaultAsync(t => t.Id == id1);
    var teamB = await context.Teams.FirstOrDefaultAsync(t => t.Id == id2);

    if (teamA != null && teamB != null)
    {
        var match2 = new Match
        {
            HomeTeam = teamA,
            AwayTeam = teamB,
            TicketPrice = 200,
            Date = DateTime.Now
        };
        await context.Matches.AddAsync(match2);
        await context.SaveChangesAsync();
    }

}
await create_many_to_many_relationship();


async Task<List<League>> getMany()
{
    return await context.Leagues.ToListAsync();
}
var allLeagues = await getMany();
Console.WriteLine("All Leagues:");
foreach (var league in allLeagues)
{
    Console.WriteLine(league.Name);
    break;
}

async Task getSingleById(int id)
{
    //    v1
    var league1 = await context.Leagues.FindAsync(id);
    Console.WriteLine($"League 1: {league1.Name}");//nullable 
    //    v2
    var league2 = await context.Leagues.FirstOrDefaultAsync(l => l.Id == id);
    Console.WriteLine($"League 2: {league2.Name}");//nullable
    // v3
    var league3 = await context.Leagues.Where(l => l.Id == id).SingleAsync();
    Console.WriteLine($"League 3: {league3.Name}");//not nullable; throws exception if not found
    // v4
    var league4 = await context.Leagues.SingleAsync(l => l.Id == id);
    Console.WriteLine($"League 4: {league4.Name}");//not nullable; throws exception if not found

}

await getSingleById(league1Id);


// get records with related data

async Task<List<Team>> includeParentRelated()
{

    return await context.Teams.Include(t => t.League).ToListAsync();
}
var teams = await includeParentRelated();
foreach (var team in teams)
{
    Console.WriteLine($"Team: {team.Name}, League: {team.League.Name}");
    break;
}

async Task<League> includeChildRelated(int parentId)
{
    return await context.Leagues.Where(l => l.Id == parentId).Include(l => l.Teams).SingleAsync();
}
var league11 = await includeChildRelated(league1Id);
Console.WriteLine($"League: {league11.Name}");
foreach (var team in league11.Teams)
{
    Console.WriteLine($"Team: {team.Name}");
    break;
}


// many to many relationship

async Task<List<Match>> includeManyToMany()
{
    return await context.Matches.Include(m => m.HomeTeam).Include(m => m.AwayTeam).ToListAsync();
}
var matches = await includeManyToMany();
foreach (var match in matches)
{
    Console.WriteLine($"Match: {match.HomeTeam.Name} vs {match.AwayTeam.Name}");
    // break;
}