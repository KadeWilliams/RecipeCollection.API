namespace RecipeCollection.API.DTOs;
public class IngredientDto
{
    public string IngredientName { get; set; } = string.Empty;
    public string? Amount { get; set; }
    public string? Unit { get; set; }
    public bool IsOptional { get; set; }
    public string? Note { get; set; }
}
