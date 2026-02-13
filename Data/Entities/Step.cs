namespace RecipeCollection.API.Entities;

public class Step
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int StepNumber { get; set; }
    public string Description { get; set; }
}