namespace RecipeCollection.API.Entities;
public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Link { get; set; }
    public string? Cookbook { get; set; }
    public string? CookbookImageUrl { get; set; }
    public string? RecipeImageUrl { get; set; }
    public bool IsFavorite { get; set; }
    public bool Cooked { get; set; }
    public DateTime? DateCooked { get; set; }
    public string Chef { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<Step> Steps { get; set; } = new List<Step>();
    public ICollection<RecipeMealType> RecipeMealTypes { get; set; } = new List<RecipeMealType>();
    public ICollection<RecipeSeason> RecipeSeasons { get; set; } = new List<RecipeSeason>();
}

