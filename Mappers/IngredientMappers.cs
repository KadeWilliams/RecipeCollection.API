using RecipeCollection.API.DTOs;
using RecipeCollection.API.Entities;

namespace RecipeCollection.API.Mappers;
public static class IngredientMappers
{
    public static IngredientDto ToDto(this RecipeIngredient recipeIngredient)
    {
        return new IngredientDto
        {
            IngredientName = recipeIngredient.Ingredient.Name,
            Amount = recipeIngredient.Amount,
            Unit = recipeIngredient.Unit,
            IsOptional = recipeIngredient.IsOptional,
            Note = recipeIngredient.Note,
        };
    }
}
