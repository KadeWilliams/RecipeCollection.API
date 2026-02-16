namespace RecipeCollection.API.DTOs;

public class CreateRecipeRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Link { get; set; }
    public string? Cookbook { get; set; }
    public string? CookbookImageUrl { get; set; }
    public string? RecipeImageUrl { get; set; }
    public bool IsFavorite { get; set; } = false;
    public bool Cooked { get; set; } = false;
    public DateTime DateCooked { get; set; }
    public string Chef { get; set; }
    public List<string> Meals { get; set; }
    public List<string> Seasons { get; set; }
    public List<CreateIngredientDto> Ingredients { get; set; }
    public List<string> Steps { get; set; }
}

public class CreateIngredientDto
{
    public string IngredientName { get; set; }
    public string? Amount { get; set; }
    public string? Unit { get; set; }
    public bool IsOptional { get; set; } = true;
    public string? Note { get; set; }
}

public class UpdateRecipeRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Link { get; set; }
    public string? Cookbook { get; set; }
    public string? CookbookImageUrl { get; set; }
    public string? RecipeImageUrl { get; set; }
    public bool IsFavorite { get; set; } = false;
    public bool Cooked { get; set; } = false;
    public DateTime DateCooked { get; set; }
    public string Chef { get; set; }
    public List<string> Meals { get; set; }
    public List<string> Seasons { get; set; }
    public List<CreateIngredientDto> Ingredients { get; set; }
    public List<string> Steps { get; set; }

}
