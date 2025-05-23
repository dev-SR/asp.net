# Template MVC Core with Onion Arch.

- [Template MVC Core with Onion Arch.](#template-mvc-core-with-onion-arch)
  - [Setup](#setup)
    - [Create Project](#create-project)
    - [Tailwind CSS](#tailwind-css)
      - [flowbite plugins](#flowbite-plugins)
    - [Database](#database)


## Setup

### Create Project

```bash
dotnet new sln -n Learn
dotnet new gitignore

dotnet new mvc --auth Individual -n Learn.Web -o Learn.Web
dotnet new classlib -n Learn.Common -o Learn.Common
dotnet new classlib -n Learn.Entities -o Learn.Entities
dotnet new classlib -n Learn.Repository -o Learn.Repository
dotnet new classlib -n Learn.Services -o Learn.Services

dotnet sln Learn.sln add Learn.Common/
dotnet sln Learn.sln add Learn.Entities/
dotnet sln Learn.sln add Learn.Repository/
dotnet sln Learn.sln add Learn.Services/
dotnet sln Learn.sln add Learn.Web/

// add reference 
dotnet add Learn.Entities/ reference Learn.Common/

dotnet add Learn.Repository/ reference Learn.Common/
dotnet add Learn.Repository/ reference Learn.Entities/

dotnet add Learn.Services/ reference Learn.Common/
dotnet add Learn.Services/ reference Learn.Repository/
dotnet add Learn.Services/ reference Learn.Entities/

dotnet add Learn.Web/ reference Learn.Services/

dotnet build Learn.sln
```

### Tailwind CSS

```bash
pnpm add tailwindcss
npx tailwindcss init 
```

`tailwind.config.js`

```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
	content: [
		'./Views/**/*.cshtml',
		// './Pages/**/*.cshtml',
		// './**/*.{razor,html,cshtml}',
	],
	theme: {
		extend: {}
	},
	plugins: []
};

```


`package.json`

```json
  "scripts": {
    "ts:build": "npx tailwindcss -i ./wwwroot/css/tailwind.in.css -o ./wwwroot/css/tailwind.out.css --minify",
    "ts:watch": "npx tailwindcss -i ./wwwroot/css/tailwind.in.css -o ./wwwroot/css/tailwind.out.css --minify --watch"
  },
```

`.\wwwroot\css\tailwind.in.css`

```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

Add a CSS reference to the host file in the `_Layout.cshtml`:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <!-- ... -->
    <link rel="stylesheet" href="~/css/tailwind.out.css" asp-append-version="true" />
</head>
<!--... -->
```

#### flowbite plugins

First, you need to install Flowbite via NPM: `pnpm add flowbite`

Require Flowbite in the Tailwind configuration file as a plugin:

```javascript
module.exports = {
  // other options
  plugins: [
    require('flowbite/plugin')
  ],
}
```

Add the Flowbite source files to the content module to start applying utility classes from the interactive UI components such as the dropdowns, modals, and navbars:

```javascript
module.exports = {
  content: [
    // other files...
    "./node_modules/flowbite/**/*.js"
  ],
...
}
```

Add a script tag with this path before the end of the body tag in the `_Layout.cshtml` page:

```html
<!-- ... -->
    <script src="https://cdn.jsdelivr.net/npm/flowbite@2.5.2/dist/flowbite.min.js"></script>
  </body>
</html>
```

You have successfully installed Flowbite and can start using the components to build your  project.


### Database

Install below packages in corresponding project:


```bash
cd Learn.Entities
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

cd Learn.Repository
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

cd Learn.Web
dotnet add package Microsoft.EntityFrameworkCore.Design

```

Define application context:

`Learn.Entities\Models\AppUser.cs`

```csharp
using Microsoft.AspNetCore.Identity;
namespace Learn.Entities.Models;

public class AppUser : IdentityUser{}
```


`Learn.Repository\Data\ApplicationDbContext.cs`

```csharp
using Learn.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Learn.Repository.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestModel> TestModels { get; set; }
}
```



`Learn.Web\Extensions\EFCoreExtensions.cs`

```csharp
using System;
using Learn.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace Learn.Web.Extensions;


public static class EFCoreExtensions
{
    // ENABLE RUNTIME DATABASE CONTEXT
    public static void InjectDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(opts =>
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
            >>[db context and startup project in different folders]
                cd [root]
                dotnet ef migrations add firstMigration --project Repository --startup-project API 
                (or) dotnet ef migrations add firstMigration -p Repository -s API -o Data/Migrations
             */
        }
        );
    }

    // APPLY MIGRATIONS
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        Console.WriteLine(">>>Applying Migrations");
        try
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context?.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($">>>An error occurred while applying migrations: {ex.Message}");
        }
    }
}
```

Configure identity:

`Learn.Web\Extensions\IdentityExtensions.cs`

```csharp
using Learn.Entities.Models;
using Learn.Repository.Data;

namespace Learn.Web.Extensions;

public static class IdentityExtensions{

    public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
    {
        services
        .AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
    {
        app.UseAuthentication();//MUST BE BEFORE AUTHORIZATION
        app.UseAuthorization();
        return app;
    }
}
```

`Learn.Web\Views\Shared\_LoginPartial.cshtml`

```cshtml
@using Microsoft.AspNetCore.Identity
@using Learn.Entities.Models;
@inject SignInManager<AppUser> SignInManager
@inject UserManager<AppUser> UserManager

<!-- .... -->
```

`Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);
// Load environment variables from the .env file
Env.Load();
// Add services to the container.
// enable db context
builder.Services.InjectDbContext();
// enable identity auth
builder.Services.AddIdentityHandlersAndStores();

var app = builder.Build();

// Applying Migrations or updating the database at runtime 
app.ApplyMigrations();
app.AddIdentityAuthMiddlewares();//NEW
```

Applying Migrations and updating the database via cli;

```bash
dotnet ef migrations add firstMigration --project Learn.Repository --startup-project Learn.Web
dotnet ef database update --project Learn.Repository --startup-project Learn.Web
```