using RecipeCollection.API.Data.Entities;

namespace RecipeCollection.API.Entities;

public class RecipeMealType
{
    public Recipe Recipe { get; set; }
    public MealType MealType { get; set; }
}
