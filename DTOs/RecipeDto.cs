namespace RecipeCollection.API.DTOs;

public class RecipeDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Link { get; set; }
    public string? RecipeImageUrl { get; set; }
    public bool IsFavorite { get; set; }
    public bool Cooked { get; set; }
    public DateTime? DateCooked { get; set; }
    public string Chef { get; set; } = string.Empty;
    public List<string> Meals { get; set; } = new();
    public List<string> Seasons { get; set; } = new();
    public List<IngredientDto> Ingredients { get; set; } = new();
    public List<string> Steps { get; set; } = new();
}