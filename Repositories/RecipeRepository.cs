using Dapper;
using RecipeCollection.API.Configuration;
using RecipeCollection.API.Entities;

namespace RecipeCollection.API.Repositories;
public class RecipeRepository : IRecipeRepository
{
    private readonly IDbConnectionFactory _context;

    public RecipeRepository(IDbConnectionFactory context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        using var conn = _context.GetConnection();
        return await conn.QueryAsync<Recipe>("SELECT * FROM recipe");
        //return await _context.Recipes
        //    .Include(r => r.RecipeIngredients)
        //        .ThenInclude(ri => ri.Ingredient)
        //    .Include(r => r.Steps)
        //    .Include(r => r.RecipeMealTypes)
        //        .ThenInclude(rmt => rmt.MealType)
        //    .Include(r => r.RecipeSeasons)
        //        .ThenInclude(rs => rs.Season)
        //    .ToListAsync();
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        const string sql = @"SELECT * FROM recipe WHERE id = @id";
        using var conn = _context.GetConnection();
        var recipe = await conn.QueryFirstOrDefaultAsync<Recipe>
        (
            sql,
            new { id }
        );

        if (recipe == null) return null;

        const string sql2 = @"
            SELECT ri.*, i.* FROM recipe_ingredient ri INNER JOIN ingredient i on ri.ingredient_id = i.id WHERE ri.recipe_id = @RecipeId 
        ";
        var ingredients = await conn.QueryAsync<RecipeIngredient, Ingredient, RecipeIngredient>
        (
            sql2,
            (ri, i) =>
            {
                ri.Ingredient = i;
                return ri;
            },
            new { RecipeId = id },
            splitOn: "Id"
        );
        recipe.RecipeIngredients = ingredients.ToList();
        return recipe;
        //return await _context.Recipes
        //    .Include(r => r.RecipeIngredients)
        //        .ThenInclude(ri => ri.Ingredient)
        //    .Include(r => r.Steps.OrderBy(s => s.StepNumber))
        //    .Include(r => r.RecipeMealTypes)
        //        .ThenInclude(rmt => rmt.MealType)
        //    .Include(r => r.RecipeSeasons)
        //        .ThenInclude(rs => rs.Season)
        //    .FirstOrDefaultAsync(r => r.Id == id);

    }

    public async Task<Recipe> AddAsync(Recipe recipe)
    {
        throw new NotImplementedException();
        //_context.Recipes.Add(recipe);
        //await _context.SaveChangesAsync();
        //return recipe;
    }
}
