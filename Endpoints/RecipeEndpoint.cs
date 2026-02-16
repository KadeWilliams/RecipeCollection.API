using RecipeCollection.API.DTOs;
using RecipeCollection.API.Mappers;
using RecipeCollection.API.Repositories;

namespace RecipeCollection.API.Endpoints;

public static class RecipeEndpoints
{
    public static void MapRecipeEndpoints(this WebApplication app)
    {
        RouteGroupBuilder? group = app.MapGroup("/api/recipes");

        group.MapGet("/", async (IRecipeRepository repo) =>
        {
            var recipes = await repo.GetAllAsync();
            return Results.Ok(recipes.Select(r => r.ToDto()));
        });

        group.MapGet("/{id}", async (int id, IRecipeRepository repo) =>
        {
            var recipe = await repo.GetByIdAsync(id);
            return recipe is null ? Results.NotFound() : Results.Ok(recipe.ToDto());
        });

        /*
        group.MapPost("/", async (CreateRecipeRequest request, IRecipeRepository repo) =>
        {
            var recipe = request.ToEntity();
            await repo.AddAsync(recipe);
            return Results.Created($"/api/recipes/{recipe.Id}", recipe.ToDto());
        });
        */
        group.MapPost("/", async (CreateRecipeRequest request, IRecipeRepository repo) =>
        {
            var recipe = await repo.CreateAsync(request);
            return Results.Created($"/api/recipes/{recipe.Id}", recipe.ToDto());
        });

        group.MapPut("/", async (UpdateRecipeRequest request, IRecipeRepository repo) =>
        {
            var recipe = await repo.UpdateAsync(request);
            return Results.Accepted($"/api/recipes/{recipe.Id}", recipe.ToDto());
        });

        group.MapDelete("/{id}", async (int id, IRecipeRepository repo) =>
        {
            var deleted = await repo.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}