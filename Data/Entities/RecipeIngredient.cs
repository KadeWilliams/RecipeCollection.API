namespace RecipeCollection.API.Entities;

public class RecipeIngredient
{
    public string RecipeId { get; set; }
    public string IngredientId { get; set; }

    public string Amount { get; set; }
    public string Unit { get; set; }
    public bool IsOptional { get; set; }
    public string Note { get; set; }

    // Navigation property to get the ingredient name
    public Ingredient Ingredient { get; set; } = null;
}
