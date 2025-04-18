# Misc web project setups

- [Misc web project setups](#misc-web-project-setups)
  - [MVC webapi](#mvc-webapi)
    - [Building and running project with dotnet cli](#building-and-running-project-with-dotnet-cli)
    - [API DOC setup](#api-doc-setup)
    - [Adding Scaler APi Doc](#adding-scaler-api-doc)
    - [LoggerService](#loggerservice)
      - [Creating the ILoggerManager Interface and Installing NLog](#creating-the-iloggermanager-interface-and-installing-nlog)
      - [Implementing the Interface and NIog.Config File](#implementing-the-interface-and-niogconfig-file)
      - [Configuring Logger Service for Logging Messages](#configuring-logger-service-for-logging-messages)
      - [Logger Service Usage](#logger-service-usage)
    - [AutoMapper](#automapper)
      - [**Basic Example**](#basic-example)
      - [**Custom Mapping Example**](#custom-mapping-example)
      - [**Mapping Nested Objects**](#mapping-nested-objects)
    - [**Mapping Collections**](#mapping-collections)
    - [Database Connection and Migrations](#database-connection-and-migrations)
      - [Install `DotNetEnv` package](#install-dotnetenv-package)
      - [Creating Models](#creating-models)
      - [DB Context Class](#db-context-class)
      - [Generating Migrations](#generating-migrations)
      - [Applying Migrations or updating the database at runtime](#applying-migrations-or-updating-the-database-at-runtime)
      - [Data seeding...](#data-seeding)
        - [With Configuration Files](#with-configuration-files)
        - [With `Bogus` Library](#with-bogus-library)
        - [Suppress PendingModelChangesWarning](#suppress-pendingmodelchangeswarning)


## MVC webapi

### Building and running project with dotnet cli

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

### API DOC setup

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
        options.OperationSorter = OperationSorter.Method;

        //...
    });
```




### LoggerService

- Create class lib project called `Contracts`.
- Create `LoggerService`, we are going to use to write our logger logic in.
- In the `LoggerService` project, add a reference to the `Contracts` project
- Then, in the main project - `API` , add a reference to `LoggerService`. Since `Contracts` is referenced through `LoggerService`, it will also be available in the main project.

#### Creating the ILoggerManager Interface and Installing NLog

- create an interface named `ILoggerManager` inside the Contracts project
- install the `NLog.Extensions.Logging`  in our `LoggerService` project.

#### Implementing the Interface and NIog.Config File

- In the LoggerService project, we are going to create a new class: `LoggerManager`

```csharp
using System;
using Contracts;
using NLog;

namespace LoggerService;

public class LoggerManager : ILoggerManager
{
    private static ILogger logger = LogManager.GetCurrentClassLogger();
    public LoggerManager()
    {
    }
    public void LogDebug(string message) => logger.Debug(message);
    public void LogError(string message) => logger.Error(message);
    public void LogInfo(string message) => logger.Info(message);
    public void LogWarn(string message) => logger.Warn(message);
}
```


NLog needs to have information about where to put log files on the file system, what the name of these files will be, and what is the minimum level of logging that we want.


We are going to define all these constants in a text file **in the main project** and name it `nlog.config`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog   xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
        autoReload="true"
        internalLogLevel="Trace"
        internalLogFile="./internal_logs\internallog.txt">
    <!-- Define targets -->
    <targets>
        <!-- Log to a file -->
        <!-- <target name="logfile" xsi:type="File" 
        fileName="${currentdir}\log\nlog-${shortdate}.log"
        layout="${longdate} ${uppercase:${level}} ${message}"/> -->
        <!-- Log to console -->
        <target name="console" xsi:type="Console" layout="${longdate} ${uppercase:${level}} ${message}"/>
    </targets>
    <rules>
        <!-- <logger name="*" minlevel="Trace" writeTo="logfile" /> -->
        <!-- All messages with a minimum log level of Debug or higher are written to the Console -->
        <logger name="*" minlevel="Debug" writeTo="console" />
    </rules>
</nlog>
<!-- 
https://ironpdf.com/blog/net-help/nlog-csharp-guide/
NLog supports several logging levels, each with its own significance:
Trace: The most detailed level, typically used for diagnostic purposes.
Debug: Used for debugging information that can be helpful during development.
Info: General information about the application's operation.
Warn: Indicates a potential issue that does not disrupt the application.
Error: Indicates a failure that should be investigated but doesn't necessarily crash the application.
Fatal: Indicates a critical failure that should be addressed immediately.
 -->
```

#### Configuring Logger Service for Logging Messages

Setting up the configuration for a logger service is quite easy. First, we need to update the `Program` class and include the path to the configuration file for the NLog configuration:

```csharp
using NLog;
//...
var builder = WebApplication.CreateBuilder(args);

var nlogConfigFilePath = string.Concat(Directory.GetCurrentDirectory(), "\\nlog.config");
LogManager.Setup().LoadConfigurationFromFile(nlogConfigFilePath);

builder.Services.ConfigureCors();
//...
```


add the logger service inside the .NET Core’s IOC container. So, let’s add a new method in the `ServiceExtensions` class:

`api\Extensions\ServiceExtensions.cs`

```csharp
public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();
```

And after that, we need to modify the Program class to include our newly created extension method:

`Program.cs`

```csharp
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
```

#### Logger Service Usage

To test our logger service, we are going to use the default `WeatherForecastController`. You can find it in the main project in the `Controllers` folder. It comes with the ASP.NET Core Web API template.

```csharp
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly ILoggerManager _logger;
    public WeatherForecastController(ILoggerManager logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInfo("Here is info message from our values controller.");
        _logger.LogDebug("Here is debug message from our values controller.");
        _logger.LogWarn("Here is warn message from our values controller.");
        _logger.LogError("Here is an error message from our values controller.");

        return ...;
    }
}
```

### AutoMapper

is a library in C# designed to simplify object-to-object mapping. It's especially useful when you need to transform data from one object type to another, for instance, mapping between DTOs (Data Transfer Objects) and domain models. It helps reduce boilerplate code and ensures a consistent mapping process.

**How AutoMapper Works**
1. **Configuration**: Define the mappings between source and destination types.
2. **Mapping**: Use the mapper to map objects of the source type to the destination type.
3. **Validation**: AutoMapper checks that all mappings are correctly defined at runtime.

Install AutoMapper using NuGet:
```bash
Install-Package AutoMapper
```
#### **Basic Example**

**Step 1. Defining the Models**

```csharp
public class Source
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Destination
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

**Step 2. Configuring AutoMapper**

```csharp
using AutoMapper;

var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Source, Destination>();
});

IMapper mapper = config.CreateMapper();
```

*Step 3. Mapping Objects**

```csharp
var source = new Source
{
    FirstName = "John",
    LastName = "Doe"
};

var destination = mapper.Map<Destination>(source);

Console.WriteLine($"FirstName: {destination.FirstName}, LastName: {destination.LastName}");
```

**Output**:
```
FirstName: John, LastName: Doe
```



#### **Custom Mapping Example**

**1. Customizing Properties**
```csharp
cfg.CreateMap<Source, Destination>()
   .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.ToUpper()));
```

 **2. Using Custom Mapping**
```csharp
var destination = mapper.Map<Destination>(source);

Console.WriteLine($"FirstName: {destination.FirstName}, LastName: {destination.LastName}");
```

**Output**:
```
FirstName: JOHN, LastName: Doe
```

---

#### **Mapping Nested Objects**

```csharp
public class SourceNested
{
    public string Name { get; set; }
    public Address Address { get; set; }
}

    public class Address
    {
        public string City { get; set; }
    }

public class Destination
{
    public string Name { get; set; }
    public string City { get; set; }
}
```

**Configuring Mapping**

```csharp
cfg.CreateMap<SourceNested, Destination>()
   .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));
```

---

### **Mapping Collections**

```csharp
List<Source> sources = new List<Source>
{
    new Source { FirstName = "John", LastName = "Doe" },
    new Source { FirstName = "Jane", LastName = "Smith" }
};

List<Destination> destinations = mapper.Map<List<Destination>>(sources);
```


### Database Connection and Migrations

#### Install `DotNetEnv` package

- Install the `DotNetEnv ` package in the main project - `API`:

```bash
cd api
dotnet add package DotNetEnv 
```

- Create a new file named `.env` in the root of the main project - `API` and add the following content:

```bash
DATABASE_PROVIDER="sqlite"
DATABASE_URL="Data Source=sqlite.db"
```

- Load .env file and access the environment variables in the `Program.cs` file:
  
```csharp
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Load environment variables from the .env file
Env.Load();
// accessing environment variables
// through the System.Environment class
string? DATABASE_URL = Environment.GetEnvironmentVariable("DATABASE_URL");
Console.WriteLine(DATABASE_URL);
// or through the DotNetEnv.Env helper class
string? DATABASE_PROVIDER = Env.GetString("DATABASE_PROVIDER");
Console.WriteLine(DATABASE_PROVIDER);
```
  
`Load()` will automatically look for a .env file in the current directory by default

#### Creating Models

- Create a new **Class Library project** named `Entities`.
- Inside it, we are going to create a **folder** named `Models`
- In the `Models` folder we are going to create two classes - i.e. `Company` and `Employee`

#### DB Context Class

- create another new **Class Library project** named `Repository`.
- add the `Repository` project’s reference into the main project - `API`.

`API\API.csproj`

```jsx
<ItemGroup>
    <ProjectReference Include="..\Repository\Repository.csproj" />
</ItemGroup>
```

- add project reference of `Entities` in the `Repository` project as follows:

`Repository\Repository.csproj`

```jsx
  <ItemGroup>
    <ProjectReference Include="..\Entities\Entities.csproj" />
  </ItemGroup>
```

- When using separate project for repository, install below packages in the `Repository` project:

```bash
cd Repository
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

and in the main project - `API`:

```bash
cd api
dotnet add package Microsoft.EntityFrameworkCore.Design
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

#### Generating Migrations

Cerate an extension method in the `ServiceExtensions` class to configure the database connection:

```csharp
namespace api.Extensions.ServiceExtensions;
public static class ServiceExtensions
{
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
}
```

Then, call this method in the `Program` class:

```csharp
builder.Services.ConfigureSqlContext();
```

Since, db context and startup project are in different folders, we have to specify the project and startup project while creating migrations.

```bash
cd root
dotnet ef migrations add firstMigration --project Repository --startup-project API
```

This will create a new folder named `Migrations` in the `Repository` project with the migration files.

```bash
Proj/
├── WebAPISolution.sln
├── api/
│   ├── API.csproj
├── Repository/
│   ├── Repository.csproj
│   ├── Migrations
│       ├── xxxxxxxxxxxx_firstMigration.cs
```

- Before, Executing migrations, make sure you have installed the `dotnet-ef` tool globally:

```bash
dotnet tool list --global
dotnet tool install --global dotnet-ef
```


#### Applying Migrations or updating the database at runtime

```csharp
var app = builder.Build();
app.Services.CreateScope().ServiceProvider.GetRequiredService<RepositoryContext>().Database.Migrate();
```

Or, crate a new extension method in the `ServiceExtensions` class:

```csharp
namespace api.Extensions.ServiceExtensions;
public static class ServiceExtensions{
    // ENABLE RUNTIME DATABASE CONTEXT
    public static void ConfigureSqlContext(this IServiceCollection services){
       //...
    }

    // APPLY MIGRATIONS
    public static void ApplyMigrations(this IApplicationBuilder app){
        Console.WriteLine(">>>Applying Migrations");
        try{
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<RepositoryContext>();
                context?.Database.Migrate();
            }
        }
        catch (Exception ex){
            Console.WriteLine($">>>An error occurred while applying migrations: {ex.Message}");
        }
    }
}
```

Then, call this method in the `Program` class:

```csharp
builder.Services.ConfigureSqlContext();
var app = builder.Build();
app.ApplyMigrations();
```

This will apply the migrations at runtime and update the database.

#### Data seeding...


##### With Configuration Files

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

Create new migration and update the database:

```bash
cd root
dotnet ef migrations add SeedCompanies --project Repository --startup-project API
dotnet ef database update --project Repository --startup-project API
## or
cd api
dotnet run # to apply migrations at runtime
```

##### With `Bogus` Library

- Install the `Bogus` package in the `Repository` project:

```bash
cd Repository
dotnet add package Bogus
```

- Create a new folder named `SeedData` in the `Repository` project and add a new class named `SeedData`:

```csharp
using Bogus;
using Entities.Models;

namespace Repository.Seeding
{
    public static class SeedData
    {
        public static List<Company> Companies { get; private set; } = new List<Company>();
        public static List<Employee> Employees { get; private set; } = new List<Employee>();

        public static void SeedCompanies(int companyCount)
        {
            var companyFaker = new Faker<Company>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.Country, f => f.Address.Country());

            Companies = companyFaker.Generate(companyCount);
        }

        public static void SeedEmployees(int employeeCount)
        {
            if (Companies.Count == 0)
                throw new InvalidOperationException("Companies must be seeded before employees.");

            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Age, f => f.Random.Int(20, 60))
                .RuleFor(e => e.Position, f => f.Name.JobTitle())
                .RuleFor(e => e.CompanyId, f => f.PickRandom(Companies).Id);

            Employees = employeeFaker.Generate(employeeCount);
        }
    }
}

```

In `RepositoryContext` class, add a new method to seed the data:

```csharp
public class RepositoryContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed the data
        SeedData.SeedCompanies(10);  // Generate 10 companies
        SeedData.SeedEmployees(50); // Generate 50 employees

        // Apply the seed data
        modelBuilder.Entity<Company>().HasData(SeedData.Companies);
        modelBuilder.Entity<Employee>().HasData(SeedData.Employees);
    }
    //..
}
```

##### Suppress PendingModelChangesWarning

```csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

```
