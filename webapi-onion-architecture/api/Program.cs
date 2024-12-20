using Scalar.AspNetCore;
using api.Extensions.ServiceExtensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using API;
using Microsoft.AspNetCore.Mvc;
using Entities.ErrorModel;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var nlogConfigFilePath = string.Concat(Directory.GetCurrentDirectory(), "\\nlog.config");
LogManager.Setup().LoadConfigurationFromFile(nlogConfigFilePath);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(config);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// ....
builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
                .ConfigureApiBehaviorOptions(options =>
                {
                    // options.SuppressModelStateInvalidFilter = true;
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        // Console.WriteLine("Invoked");
                        var errors = context.ModelState
                            .Where(ms => ms.Value?.Errors.Count > 0)
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? new List<string>()
                            );

                        var errorDetails = new ErrorDetails
                        {
                            StatusCode = 400,
                            Message = "One or more validation errors occurred.",
                            Errors = errors
                        };

                        return new BadRequestObjectResult(errorDetails);
                    };

                });

builder.Services.AddOpenApi();


var app = builder.Build();
// app.ApplyMigration();



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
        options.OperationSorter = OperationSorter.Method;
    });
}
app.UseExceptionHandler(opt => { });
if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
