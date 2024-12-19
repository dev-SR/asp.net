
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

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(opts =>
        {

            var database = configuration.GetConnectionString("Database");
            if (database == "sqlite")
            {
                opts.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            }
            /* 
            cd root
            dotnet ef migrations add firstMigration --project Repository --startup-project API
            dotnet ef migrations add firstMigration -p Repository -s API
            dotnet ef database update -p Repository -s API
             */
        }
        );
    }


    public static void ApplyMigration(this WebApplication app)
    {

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<RepositoryContext>();
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                throw new Exception($"Migration ERROR {e.Message}");
            }
        }
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
                        services.AddScoped<IRepositoryManager, RepositoryManager>();
    public static void ConfigureServiceManager(this IServiceCollection services) =>
                        services.AddScoped<IServiceManager, ServiceManager>();
}