
using Contracts;
using Logger.Contract;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace api.Extensions.ServiceExtensions;
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

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    // ENABLE RUNTIME DATABASE CONTEXT
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
                // opts.UseSqlServer(ConnectionString);
            }
            /* 
            To Create New Migration 
            >>[db context and startup project in the same folder]
                cd [root]
                dotnet ef migrations add firstMigration --startup-project API
                (or) dotnet ef migrations add firstMigration -s API
            
            >>[db context and startup project in different folders]
                cd [root]
                dotnet ef migrations add firstMigration --project Repository --startup-project API 
                (or) dotnet ef migrations add firstMigration -p Repository -s API -o Data/Migrations
             */
        }
        );
    }

    // APPLY MIGRATIONS AT RUNTIME
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        Console.WriteLine(">>>Applying Migrations");
        try
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<RepositoryContext>();
                context?.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($">>>An error occurred while applying migrations: {ex.Message}");
        }
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
                        services.AddScoped<IRepositoryManager, RepositoryManager>();
    public static void ConfigureServiceManager(this IServiceCollection services) =>
                        services.AddScoped<IServiceManager, ServiceManager>();
}