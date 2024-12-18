using Scalar.AspNetCore;
using api.Extensions.ServiceExtensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureSqlContext(config);
// ....
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
app.ApplyMigration();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("First APi");
        options.WithTheme(ScalarTheme.DeepSpace);
        options.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
        options.WithModels(false);
    });
}
else { app.UseHsts(); }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
