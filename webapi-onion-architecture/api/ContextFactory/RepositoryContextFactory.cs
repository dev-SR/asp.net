// using System;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Repository;

// namespace API.ContextFactory;


// public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
// {
//     public RepositoryContext CreateDbContext(string[] args)
//     {
//         // IDesignTimeDbContextFactory is used usually when you execute EF Core commands like Add-Migration, Update-Database, and so on
//         // So it is usually your local development machine environment


//         // Prepare configuration builder
//         var configuration = new ConfigurationBuilder()
//                                 .SetBasePath(Directory.GetCurrentDirectory())
//                                 .AddJsonFile("appsettings.json")
//                                 .Build();
//         var builder = new DbContextOptionsBuilder<RepositoryContext>();
//         var database = configuration.GetConnectionString("Database");
//         if (database == "sqlite")
//         {
//             builder.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
//                                 b => b.MigrationsAssembly("API"));//puts migration in API project
//         }
//         else
//         {
//             // builder.useX(configuration.GetConnectionString("DefaultConnection"),
//             //   b => b.MigrationsAssembly("API"));//puts migration in API project
//         }
//         /* 
//                             cd api 
//                             dotnet ef migrations add firstMigration
//                             dotnet ef database update
//         */

//         return new RepositoryContext(builder.Options);
//     }
// }
