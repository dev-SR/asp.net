# REST API: ASP.NET CORE mvc webpi

- [REST API: ASP.NET CORE mvc webpi](#rest-api-aspnet-core-mvc-webpi)
  - [Building and running project with dotnet cli](#building-and-running-project-with-dotnet-cli)
  - [API DOC setup](#api-doc-setup)
    - [Adding Scaler APi Doc](#adding-scaler-api-doc)
  - [LoggerService](#loggerservice)
    - [Creating the ILoggerManager Interface and Installing NLog](#creating-the-iloggermanager-interface-and-installing-nlog)
    - [Implementing the Interface and NIog.Config File](#implementing-the-interface-and-niogconfig-file)
    - [Configuring Logger Service for Logging Messages](#configuring-logger-service-for-logging-messages)
    - [Logger Service Testing](#logger-service-testing)
  - [Onion architecture](#onion-architecture)
    - [Creating Models](#creating-models)
    - [Context Class and the Database Connection](#context-class-and-the-database-connection)
    - [Data seeding...](#data-seeding)
    - [Repository Pattern Logic](#repository-pattern-logic)
    - [Simplified Repository Pattern Logic](#simplified-repository-pattern-logic)
      - [Repository User Interfaces and Classes](#repository-user-interfaces-and-classes)
      - [Creating a Repository Manager](#creating-a-repository-manager)
    - [Registering RepositoryContext at a Runtime](#registering-repositorycontext-at-a-runtime)
    - [Adding a Service Layer](#adding-a-service-layer)
    - [Controllers and Routing in WEB API](#controllers-and-routing-in-web-api)



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

## LoggerService

- Create class lib project called `Contracts`.
- Create `LoggerService`, we are going to use to write our logger logic in.
- In the `LoggerService` project, add a reference to the `Contracts` project
- Then, in the main project - `API` , add a reference to `LoggerService`. Since `Contracts` is referenced through `LoggerService`, it will also be available in the main project.

### Creating the ILoggerManager Interface and Installing NLog

- create an interface named `ILoggerManager` inside the Contracts project
- install the `NLog.Extensions.Logging`  in our `LoggerService` project.

### Implementing the Interface and NIog.Config File

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
        internalLogFile=".\internal_logs\internallog.txt">
    <targets>
        <target name="logfile" xsi:type="File" fileName=".\logs\${shortdate}_logfile.txt" layout="${longdate} ${level:uppercase=true} ${message}"/>
    </targets>
    <rules>
        <logger name="*" minlevel="Debug" writeTo="logfile" />
    </rules>
</nlog>
```

### Configuring Logger Service for Logging Messages

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


### Logger Service Testing

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



## Onion architecture

### Creating Models

- Create a new **Class Library project** named `Entities`.
- Inside it, we are going to create a **folder** named `Models`
- In the `Models` folder we are going to create two classes - i.e. `Company` and `Employee`

### Context Class and the Database Connection

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

```jsx
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
  </ItemGroup>
```

```jsx
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


### Repository Pattern Logic

### Simplified Repository Pattern Logic

The **Repository Pattern** involves creating a **generic repository** to handle basic CRUD operations. This allows you to reuse these methods across different repository classes in your project. 

Here’s the flow:

1. **Generic Repository**: Contains reusable CRUD methods applicable to all data types.
2. **Specific Repository Classes**: Extend the generic repository to handle entity-specific logic.
3. **Service Wrapper**: A wrapper class integrates all repository classes, enabling centralized access.
4. **Dependency Injection**: The service wrapper is registered in the **DI container**, allowing controllers to access any repository through a single, reusable service instance. 

This setup ensures clean code, scalability, and centralized control over data operations.

- create a new Class Library (C#) project named `Contracts`, we are going to keep our interfaces.
- Add a reference to the `Contracts` project in the `Repository` project. 
  - Note, Since `Contracts` is referenced in `Repository`, **there's no need to add it separately in the `API`** project for dependency injection (`services.AddScoped<IRepositoryManager, RepositoryManager>();`) because the `Repository` project is already referenced in `API` and `Contracts` project in the `Repository`.
- Now, create an interface for the repository inside the Contracts project:

`Contracts\IRepositoryBase.cs`

```csharp
using System.Linq.Expressions;
namespace Contracts;
public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
    bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
```

Right after the interface creation, in the `Repository` project, we are going to create an abstract class `RepositoryBase` — which is going to implement the `IRepositoryBase` interface:


```csharp
using System;
using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;
    public RepositoryBase(RepositoryContext repositoryContext) => RepositoryContext = repositoryContext;

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>()
                .AsNoTracking() :
            RepositoryContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking() :
            RepositoryContext.Set<T>()
                .Where(expression);

    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
}
```

Generic type `T`. This type T gives even more reusability to the RepositoryBase class as we don’t have to specify the exact model (class) right now for the RepositoryBase to work with.

Moreover, we can see the `trackChanges` parameter. We are going to use it to improve our read-only query performance. When it’s set to false, we attach the AsNoTracking method to our query to inform EF Core that it
doesn’t need to track changes for the required entities. This greatly improves the speed of a query.

#### Repository User Interfaces and Classes

Now that we have the `RepositoryBase` class, let’s create the user classes that will inherit this abstract class.
By inheriting from the `RepositoryBase` class, they will have access to all the methods from it.

**Furthermore**, every user class will have its interface for additional model-specific methods. Let’s create the interfaces in the `Contracts` project for the `Company` and `Employee` classes:

`Contracts\IEmployeeRepository.cs`

```csharp
namespace Contracts;
public interface IEmployeeRepository{}
```

`Contracts\ICompanyRepository.cs`

```csharp
namespace Contracts;
public interface ICompanyRepository{}
```


After this, we can create**repository user classes** in the `Repository` project - (`CompanyRepository`, `EmployeeRepository`)

`Repository\CompanyRepository.cs`:

```csharp
using Contracts;
using Entities.Models;
namespace Repository;
public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
}
```

`Repository\EmployeeRepository.cs`:


```csharp
using Contracts;
using Entities.Models;
namespace Repository;
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
}
```

After these steps, we are finished creating the repository and repository user classes. 
> But there are still more things to do.

#### Creating a Repository Manager

APIs often return data from multiple resources, such as all companies and employees over 30. Managing this with multiple repository classes can become complex, what if we need the combined logic of five or even more different classes?


To simplify, we'll create a **repository manager class** that **instantiates repository user classes** and **registers them in the dependency injection container**.

But we are also missing one important part. We have the `Create`, `Update`, and `Delete` methods in the `RepositoryBase` class, but they won’t make any change in the database until we call the `SaveChanges` method. Our repository manager class will handle that as well.

That said, let’s get to it and create a new interface in the `Contract` project:

`Contracts\IRepositoryManager.cs`

```csharp
namespace Contracts;

public interface IRepositoryManager
{
    ICompanyRepository Company { get; }
    IEmployeeRepository Employee { get; }
    void Save();
}
```

And add a new class to the `Repository` project:

```csharp
using Contracts;
namespace Repository;
public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;
    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));
    }
    public ICompanyRepository Company => _companyRepository.Value;
    public IEmployeeRepository Employee => _employeeRepository.Value;
    public void Save() => _repositoryContext.SaveChanges();
}
```

Notice that the repository manager exposes concrete repositories as properties and provides a single `Save()` method for persisting all changes. This approach enables batching multiple operations, such as adding two companies, updating two employees, and deleting one company, into a single transaction. If something fails, all changes are reverted. Example:

```csharp
_repository.Company.Create(company);
_repository.Company.Create(anotherCompany);
_repository.Employee.Update(employee);
_repository.Employee.Update(anotherEmployee);
_repository.Company.Delete(oldCompany);
_repository.Save();
```

Also, The **RepositoryManager** utilizes the `Lazy` class for lazy initialization of repositories. This ensures repository instances are created only when accessed for the first time, optimizing resource usage and improving performance.



To integrate the **RepositoryManager**, register it in the main project by updating the `ServiceExtensions` and `Program` classes:

1. **Modify `ServiceExtensions`**:
   ```csharp
   public static void ConfigureRepositoryManager(this IServiceCollection services) =>
       services.AddScoped<IRepositoryManager, RepositoryManager>();
   ```

2. **Update `Program` Class**:
   Add the following line above the `AddControllers()` method:
   ```csharp
   builder.Services.ConfigureRepositoryManager();
   ``` 

This ensures the `RepositoryManager` is available as a scoped service throughout the application.

Excellent - The repository layer is prepared and ready to be used to fetch data from the database.


### Registering RepositoryContext at a Runtime

With the `RepositoryContextFactory` class, which implements the `IDesignTimeDbContextFactory` interface, we have registered our RepositoryContext class at **design time**. **This helps us find the RepositoryContext class in another project while executing migrations.**

But, as you could see, we have the `RepositoryManager` service registration, which happens at runtime, and during that registration, w**e must have `RepositoryContext` registered as well in the runtime**, so we could inject it into other services

Let’s modify the `ServiceExtensions` class

```csharp
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
```

As the final step, we have to call this method in the `Program` class:

```csharp
builder.Services.ConfigureSqlContext(builder.Configuration);
```


### Adding a Service Layer

The Service layer sits right above the Domain layer (the Contracts project is the part of the Domain layer), which means that it has a reference to the Domain layer. The Service layer will be split into two projects, `Service.Contracts` and `Service`.


Create `Service.Contracts` project (.NET Class Library) where we will hold the definitions for the service interfaces that are going to encapsulate the main business logic

Once the project is created, we are going to add three interfaces inside it.

`Service.Contracts\ICompanyService.cs`:

```csharp
namespace Service.Contracts;
public interface ICompanyService{}
```

`Service.Contracts\IEmployeeService.cs`:

```csharp
namespace Service.Contracts;
public interface IEmployeeService{}
```

`Service.Contracts\IServiceManager.cs`

```csharp
namespace Service.Contracts;
public interface IServiceManager
{
    ICompanyService CompanyService { get; }
    IEmployeeService EmployeeService { get; }
}
```


Now, we can create another project, name it `Service`, and reference the `Service.Contracts` and `Contracts` projects inside it:

`Service\Service.csproj`


```jsx
<ItemGroup>
    <ProjectReference Include="..\Contracts\Contracts.csproj" />
    <ProjectReference Include="..\Service.Contracts\Service.Contracts.csproj" />
</ItemGroup>
```

After that, we are going to create classes that will inherit from the interfaces that reside in the `Service.Contracts` project. So, let’s start with the `CompanyService` class:

`Service\CompanyService.cs`
```csharp
using Contracts;
using Service.Contracts;
namespace Service;
internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    public CompanyService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

```

To continue, let’s create a new `EmployeeService` class:

`Service\EmployeeService.cs`

```csharp
using Contracts;
using Service.Contracts;
namespace Service;
internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    public EmployeeService(IRepositoryManager repository, ILoggerManager
    logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
```


We are going to use `IRepositoryManager` to access the repository methods from each user repository class (`CompanyRepository` or
`EmployeeRepository`), and `ILoggerManager` to access the logging methods we’ve created in the second section of this book.


Finally, we are going to create the `ServiceManager` class:
`Service\ServiceManager.cs`:

```csharp
using Contracts;
using Service.Contracts;
namespace Service;
public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
    {
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, logger));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, logger));
    }
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}
```

Now, with all these in place, we have to add the reference from the `Service` project inside the main project - `API` . Since Service is already referencing `Service.Contracts`, our main project will have the same reference as well.

Now, we have to modify the `ServiceExtensions` class:

```csharp
public static void ConfigureServiceManager(this IServiceCollection services) =>
services.AddScoped<IServiceManager, ServiceManager>();
```

Then, all we have to do is to modify the `Program` class to call this extension method:

```csharp
//..
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
//..
```

### Controllers and Routing in WEB API

Another separate project for Controller logic also?

This will improve design by isolating the **presentation layer**, which serves as the entry point for system interaction (e.g., REST APIs). This separation enforces stricter rules, preventing controllers from injecting anything they want and avoiding tight coupling with other projects; our presentation layer will depend only on the `service.contracts`, thus imposing more strict rules on our controllers.

- Create new class lib project - `Presentation`

- Inside `Presentation.csproj` file, we are going to add a new framework reference so it has access to the `ControllerBase` class for our future controllers:

`Presentation\Presentation.csproj`
```jsx
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
```

- Additionally, let’s create a single class inside the Presentation project:

```csharp
namespace Presentation;
public static class AssemblyReference { }
```

It's an empty static class that we are going to use for the assembly reference inside the main project.

- The one more thing, we have to do is to reference the `Service.Contracts` project inside the Presentation project.
- Next, we have to reference the `Presentation` project inside the main project - `API`. As you can see, our presentation layer depends only on the service contracts, thus imposing more strict rules on our controllers.
- Now, we are going to delete the `Controllers` folder and the `WeatherForecast.cs` file from the main project because we are not going to need them anymore.

- Then, we have to modify the `Program.cs` file:

```csharp
builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
```


- let’s navigate to the `Presentation` project, create a new folder named `Controllers`, and
then a new class named `BaseApiController`.

`Presentation\Controllers\BaseApiController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase { }



```

this will work as our base `/api/` route.

Lets create a test controller inheriting the base

```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("test")]
[ApiController]
public class TestController : BaseController
{


    [HttpGet]
    public Person GetPerson()
    {
        var person1 = new Person("John Doe", 30);
        return person1;

    }
}

public record Person(string Name, int Age);
```
