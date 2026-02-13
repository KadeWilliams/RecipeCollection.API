namespace RecipeCollection.API.Entities;

public class RecipeIngredient
{
    public Ingredient Ingredient { get; set; }
    public string Amount { get; set; }
    public string Unit { get; set; }
    public bool IsOptional { get; set; }
    public string Note { get; set; }
}
