using API.Extensions;
using API.Services;
using API.Services.Contracts;
using Scalar.AspNetCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
// Load environment variables from the .env file
Env.Load();

// Add services to the container.

builder.Services.ConfigureCors();//new
builder.Services.ConfigureSqlContext();//new
builder.Services.AddScoped<IHeroService, HeroService>();//new

builder.Services.AddOpenApi();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.ApplyMigrations();//new
app.MapOpenApi();
app.MapScalarApiReference(options => //new
{
    options.WithTitle("First APi");
    options.WithTheme(ScalarTheme.DeepSpace);
    options.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
    options.WithModels(false);
    //...
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
