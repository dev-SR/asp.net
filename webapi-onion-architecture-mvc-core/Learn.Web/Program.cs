using DotNetEnv;
using Learn.Web.Extensions;


var builder = WebApplication.CreateBuilder(args);
// Load environment variables from the .env file
Env.Load();
// Add services to the container.
// enable db context
builder.Services.InjectDbContext();
// enable identity auth
builder.Services.AddIdentityHandlersAndStores();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// apply migrations
app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.AddIdentityAuthMiddlewares();//NEW

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
