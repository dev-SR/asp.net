# REST API: ASP.NET CORE mvc webpi

- [REST API: ASP.NET CORE mvc webpi](#rest-api-aspnet-core-mvc-webpi)
  - [Building and running project with dotnet cli](#building-and-running-project-with-dotnet-cli)
  - [API DOC setup](#api-doc-setup)
    - [Adding Scaler APi Doc](#adding-scaler-api-doc)
  - [Onion architecture](#onion-architecture)
    - [Creating Models](#creating-models)
    - [Context Class and the Database Connection](#context-class-and-the-database-connection)
    - [Data seeding...](#data-seeding)



## Building and running project with dotnet cli

- Step 1: Create a New Solution  
  
```bash
 dotnet new sln -n WebAPISolution
```

- Step 2: Create api project 

```bash
dotnet new webapi --use-controllers -n API -o api
```

`--use-controllers` required for creating mvc webapi with .NET 9.0

- Step 3: Add the project to the Solution  
```bash
dotnet sln WebAPISolution.sln add api/MyConsoleApp.csproj
# or just:
dotnet sln WebAPISolution.sln add api/
```

After running the above commands, your directory structure will look like this:  

```
Proj/
├── WebAPISolution.sln
├── api/
│   ├── API.csproj
│   ├── Program.cs
│   ├── ....
```

-  Step 4: Restore and Build the Solution  

Restore dependencies and build the solution using the following commands:  
```bash
dotnet restore
# or
dotnet restore WebAPISolution.sln
dotnet build
```

- Step 5: Run the Console Application  

**Navigate to the `api` directory** and execute the following command:  

```bash
cd api
dotnet watch run
```

## API DOC setup

> Swagger dropped in .NET 9: What are the alternatives?

- [https://youtu.be/fJWEXqGxbJg?si=RvCYIW4MSkEIIHFc](https://youtu.be/fJWEXqGxbJg?si=RvCYIW4MSkEIIHFc)
 
### Adding Scaler APi Doc 

- [https://www.nuget.org/packages/Scalar.AspNetCore](https://www.nuget.org/packages/Scalar.AspNetCore)

1. Install the package

```bash
dotnet add package Scalar.AspNetCore --version 1.2.61
```

2. Add the using directive at `Program.cs`


```csharp
using Scalar.AspNetCore;
```

3. Configure your application
   
Add the following to `Program.cs` based on your OpenAPI generator:

For .NET 9 using `Microsoft.AspNetCore.OpenApi`:


```csharp
builder.Services.AddOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();//new
}
```

That’s it! 🎉 With the default settings, you can now access the Scalar API reference at `/scalar/v1` in your browser, where v1 is the default document name.

Change Configurations:

```csharp
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("First APi");
        options.WithTheme(ScalarTheme.DeepSpace);
        options.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
        options.WithModels(false);
        //...
    });
```

## Onion architecture

### Creating Models

- Create a new **Class Library project** named `Entities`.
- Inside it, we are going to create a **folder** named `Models`
- In the `Models` folder we are going to create two classes - i.e. `Company` and `Employee`

### Context Class and the Database Connection

- create another new **Class Library project** named `Repository`.
- add the `Repository` project’s reference into the main project - `API`.

`API\API.csproj`

```tsx
<ItemGroup>
    <ProjectReference Include="..\Repository\Repository.csproj" />
</ItemGroup>
```

- add project reference of `Entities` in the `Repository` project as follows:

`Repository\Repository.csproj`

```tsx
  <ItemGroup>
    <ProjectReference Include="..\Entities\Entities.csproj" />
  </ItemGroup>
```

- Then, navigate to the root of the `Repository` project and create the `RepositoryContext` class:

```csharp
using System;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options) : base(options) { }
    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}
```

- After the class modification, let’s open the `appsettings.json` file, in the main project `API`, and add the connection string:

```json
{
 "ConnectionStrings": {
    "Database": "sqlite",
    "DefaultConnection": "Data Source=SQLLiteDatabase.db"
  }
}
```


- let’s create a new `ContextFactory` folder in the main project  `API` and inside it a new `RepositoryContextFactory` class

 Since our `RepositoryContext` class is in a `Repository` project and **not in the main one - API**, this class will help our application create a derived `DbContext` instance **during the design time** which will help us with our **migrations**:

`api\ContextFactory\RepositoryContextFactory.cs`

```csharp
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;
namespace API.ContextFactory;
public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();
        var builder = new DbContextOptionsBuilder<RepositoryContext>();
        var database = configuration.GetConnectionString("Database");
        if (database == "sqlite")
        {
            builder.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                                b => b.MigrationsAssembly("API"));//puts migration in API project
        }
        else
        {
            // builder.useX(configuration.GetConnectionString("DefaultConnection"),
            //   b => b.MigrationsAssembly("API"));//puts migration in API project
        }
        /* 
                            cd api 
                            dotnet ef migrations add firstMigration
                            dotnet ef database update
        */
        return new RepositoryContext(builder.Options);
    }
}
```

because migration assembly is not in our main project, but in the Repository project. So, we’ve used `b => b.MigrationsAssembly("API")` for the migration assembly.


- Before Executing migrations install Entity EntityFrameworkCore packages as follows:

`api\API.csproj`:

`Repository\Repository.csproj`:

```tsx
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
  </ItemGroup>
```

```tsx
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>
```

- Executing migrations:

```bash
dotnet tool list --global
dotnet tool install --global dotnet-ef
cd api 
dotnet ef migrations add firstMigration
```

With this command, we are creating migration files and we can find them in the `Migrations` folder in our main project:


```
Proj/
├── WebAPISolution.sln
├── api/
│   ├── API.csproj
│   ├── Migrations
│       ├── xxxxxxxxxxxx_firstMigration.cs
```


### Data seeding...

Once we have the database and tables created, we should populate them with some initial data. To do that, we are going to create another folder in the `Repository` project called `Configuration` and add the Configuration classes:


```csharp
using System;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Repository.Configuration;
public class CompanyConfiguration : IEntityTypeConfiguration<Company>{
    public void Configure(EntityTypeBuilder<Company> builder){
        builder.HasData(
                new Company
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "IT_Solutions Ltd",
                    Address = "583 Wall Dr. Gwynn Oak, MD 21207",
                    Country = "USA"
                },
                new Company
                {
                    Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                    Name = "Admin_Solutions Ltd",
                    Address = "312 Forest Avenue, BF 923",
                    Country = "USA"
                }
        );
    }
}
```

To invoke this configuration, we have to change the `RepositoryContext` class:

```csharp
using System;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}
```

Now, we can create and apply another migration to seed these data to the database:

```bash
cd api 
dotnet ef migrations add InitialData
dotnet ef database update
```

This will transfer all the data from our configuration files to the respective
tables.

Removing migration:

```bash
cd api
dotnet ef database drop
dotnet ef migrations remove -f
```

