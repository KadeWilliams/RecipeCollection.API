using RecipeCollection.API.DTOs;
using RecipeCollection.API.Entities;

namespace RecipeCollection.API.Mappers;
public static class RecipeMappers
{
    public static RecipeDto ToDto(this Recipe recipe)
    {
        return new RecipeDto
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Link = recipe.Link,
            RecipeImageUrl = recipe.RecipeImageUrl,
            IsFavorite = recipe.IsFavorite,
            Cooked = recipe.Cooked,
            DateCooked = recipe.DateCooked,
            Chef = recipe.Chef,
            Meals = recipe.RecipeMealTypes
                .Select(rmt => rmt.MealType.Name)
                .ToList(),
            Seasons = recipe.RecipeSeasons
                .Select(rs => rs.Season.Name)
                .ToList(),
            Ingredients = recipe.RecipeIngredients
                .Select(ri => ri.ToDto())
                .ToList(),
            Steps = recipe.Steps
                .OrderBy(s => s.StepNumber)
                .Select(s => s.Description)
                .ToList()
        };
    }
    //public static Recipe ToEntity 
}