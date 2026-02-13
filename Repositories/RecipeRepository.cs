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
        throw new NotImplementedException();
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
