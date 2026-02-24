using RecipeCollection.API.Configuration;
using RecipeCollection.API.Endpoints;
using RecipeCollection.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'Database' not found.");
builder.Services.AddSingleton<IDbConnectionFactory>(new NpgsqlConnectionFactory(connectionString));
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        //builder.AllowAnyOrigin();
        builder.WithOrigins(
                "http://localhost:3000",
                "https://recipe-collection-six.vercel.app"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowReactApp");

app.MapGet("/", () => "Hello World!");

app.MapRecipeEndpoints();

app.Run();
