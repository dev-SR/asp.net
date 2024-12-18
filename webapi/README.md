# REST API: ASP.NET CORE mvc webpi

- [REST API: ASP.NET CORE mvc webpi](#rest-api-aspnet-core-mvc-webpi)
  - [Building and running project with dotnet cli](#building-and-running-project-with-dotnet-cli)
  - [API DOC setup](#api-doc-setup)
    - [Adding Scaler APi Doc](#adding-scaler-api-doc)


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
