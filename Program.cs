using RecipeCollection.API.Configuration;
using RecipeCollection.API.Endpoints;
using RecipeCollection.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'Database' not found.");
builder.Services.AddSingleton<IDbConnectionFactory>(new NpgsqlConnectionFactory(connectionString));

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapRecipeEndpoints();

app.Run();
