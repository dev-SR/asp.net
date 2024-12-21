using System;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace API.Extensions;

public static class ServiceExtensions
{
    //Extend ConfigureCors()
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
    }

    public static void ConfigureSqlContext(this IServiceCollection services)
    {
        services.AddDbContext<RepositoryContext>(opts =>
        {
            Console.WriteLine(">>>Configuring SQL Context");

            string? DatabaseProvider = Environment.GetEnvironmentVariable("DATABASE_PROVIDER");
            string? ConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (DatabaseProvider == "sqlite")
            {
                Console.WriteLine(">>>Using SQLite");
                opts.UseSqlite(ConnectionString);

            }
            else if (DatabaseProvider == "sqlserver")
            {
                Console.WriteLine(">>>Using SQL Server");
                opts.UseSqlServer(ConnectionString);
            }
            /* 
            [db context and startup project in the same folder]
                cd [root]
                dotnet ef migrations add firstMigration -s API
                dotnet ef database update -s API
                [db context and startup project in different folders]
            cd [root]
                dotnet ef migrations add firstMigration --project Repository --startup-project API
                dotnet ef migrations add firstMigration -p Repository -s API
                dotnet ef database update -p Repository -s API
             */
        }
        );
    }
    public static void ApplyMigration(this IServiceCollection services)
    {
        try
        {
            Console.WriteLine(">>Applying Migrations");
            // Access the environment variables
            string? DatabaseProvider = Environment.GetEnvironmentVariable("DATABASE_PROVIDER");
            string? ConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

            var builder = new DbContextOptionsBuilder<RepositoryContext>();
            if (DatabaseProvider == "sqlite")
            {
                Console.WriteLine(">>>Using SQLite");
                builder.UseSqlite(ConnectionString);

            }
            else if (DatabaseProvider == "sqlserver")
            {
                Console.WriteLine(">>>Using SQL Server");
                builder.UseSqlServer(ConnectionString);
            }

            var dbContext = new RepositoryContext(builder.Options);
            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            throw new Exception($"Migration ERROR {e.Message}");
        }

    }

}
