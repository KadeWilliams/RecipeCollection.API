using RecipeCollection.API.DTOs;
using RecipeCollection.API.Entities;

namespace RecipeCollection.API.Repositories;

public interface IRecipeRepository
{
    Task<IEnumerable<Recipe>> GetAllAsync();
    Task<Recipe?> GetByIdAsync(int id);
    Task<Recipe> CreateAsync(CreateRecipeRequest recipe);
    Task<bool> DeleteAsync(int id);
}
