using System.Reflection;
using DbUp;
using Scalar.AspNetCore;

// WebRootPath: static files are served from /frontend instead of the default /wwwroot
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "frontend"
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<backend.Data.DapperContext>();
builder.Services.AddScoped<backend.Data.IProjectRepository, backend.Data.ProjectRepository>();

var app = builder.Build();

// DbUp: create the database and run the scripts in /scripts (same as the database project in lab 8)
var connectionString = app.Configuration.GetConnectionString("Default");
EnsureDatabase.For.SqlDatabase(connectionString);
var result = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build()
    .PerformUpgrade();
if (!result.Successful)
    throw result.Error;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// The frontend (/frontend) is served by the API itself
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
