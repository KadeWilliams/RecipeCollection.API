using Dapper;
using RecipeCollection.API.Configuration;
using RecipeCollection.API.Data.Entities;
using RecipeCollection.API.DTOs;
using RecipeCollection.API.Entities;

namespace RecipeCollection.API.Repositories;
public class RecipeRepository : IRecipeRepository
{
    private readonly IDbConnectionFactory _context;

    public RecipeRepository(IDbConnectionFactory context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        using var conn = _context.GetConnection();
        return await conn.QueryAsync<Recipe>("SELECT * FROM recipe");
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        using var conn = _context.GetConnection();

        const string recipeSql = @"SELECT * FROM recipe WHERE id = @id";
        var recipe = await conn.QueryFirstOrDefaultAsync<Recipe>
        (
            recipeSql,
            new { id }
        );

        if (recipe == null) return null;

        const string ingredientsSql = @"
            SELECT ri.*, i.* FROM recipe_ingredient ri INNER JOIN ingredient i on ri.ingredient_id = i.id WHERE ri.recipe_id = @RecipeId 
        ";
        var ingredients = await conn.QueryAsync<RecipeIngredient, Ingredient, RecipeIngredient>
        (
            ingredientsSql,
            (ri, i) =>
            {
                ri.Ingredient = i;
                return ri;
            },
            new { RecipeId = id },
            splitOn: "Id"
        );
        recipe.RecipeIngredients = ingredients.ToList();

        const string seasonSql = @"
            SELECT rs.*, s.id, s.name FROM recipe_season rs INNER JOIN reference.season s ON rs.season_id = s.id WHERE rs.recipe_id = @RecipeId 
        ";
        var seasons = await conn.QueryAsync<RecipeSeason, Season, RecipeSeason>
        (
            seasonSql,
            (rs, s) =>
            {
                rs.Season = s;
                return rs;
            },
            new { RecipeId = id },
            splitOn: "Id"
        );
        recipe.RecipeSeasons = seasons.ToList();

        const string mealTypeSql = @"
            SELECT *
            FROM recipe_meal_type rmt
                INNER JOIN reference.meal_type mt ON rmt.meal_type_id = mt.id
            WHERE rmt.recipe_id = @RecipeId
        ";

        var mealTypes = await conn.QueryAsync<RecipeMealType, MealType, RecipeMealType>
        (
            mealTypeSql,
            (rmt, mt) =>
            {
                rmt.MealType = mt;
                return rmt;
            },
            new { RecipeId = id },
            splitOn: "Id"
        );
        recipe.RecipeMealTypes = mealTypes.ToList();

        const string stepsSql = @"
            SELECT *
            FROM step s 
            WHERE s.recipe_id = @RecipeId
        ";

        var steps = await conn.QueryAsync<Step>
        (
            stepsSql,
            new { RecipeId = id }
        );
        recipe.Steps = steps.ToList();
        return recipe;
    }

    public async Task<Recipe> CreateAsync(CreateRecipeRequest recipeRequest)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        using var tran = conn.BeginTransaction();

        try
        {
            const string recipeSql = @"
                INSERT INTO recipe 
                (title, description, link, cookbook, cookbook_image_url,
                recipe_image_url, is_favorite, cooked, date_cooked, chef)
                VALUES
                (@Title, @Description, @Link, @Cookbook, @CookbookImageUrl,
                @RecipeImageUrl, @IsFavorite, @Cooked, @DateCooked, @Chef)
                RETURNING id
            ";

            var recipeId = await conn.ExecuteScalarAsync<int>(recipeSql, recipeRequest, tran);

            foreach (var ingredientDto in recipeRequest.Ingredients)
            {
                const string getIngredientsSql = @"
                    INSERT INTO ingredient (name)
                    Values (@Name) 
                    ON CONFLICT(NAME) DO UPDATE SET name = EXCLUDED.name
                    RETURNING id
                ";

                var ingredientId = await conn.ExecuteScalarAsync<int>
                (
                    getIngredientsSql,
                    new { Name = ingredientDto.IngredientName },
                    tran
                );

                const string linkSql = @"
                    INSERT INTO recipe_ingredient (recipe_id, ingredient_id, amount, unit, is_optional, note)
                    VALUES (@RecipeId, @IngredientId, @Amount, @Unit, @IsOptional, @Note)
                ";

                await conn.ExecuteAsync
                (
                    linkSql,
                    new
                    {
                        RecipeId = recipeId,
                        IngredientId = ingredientId,
                        ingredientDto.Amount,
                        ingredientDto.Unit,
                        ingredientDto.IsOptional,
                        ingredientDto.Note
                    },
                    tran
                );
            }

            for (int i = 0; i < recipeRequest.Steps.Count; i++)
            {
                const string stepSql = @"
                        INSERT INTO step (recipe_id, step_number, description)
                        VALUES (@RecipeId, @StepNumber, @Description)
                    ";

                await conn.ExecuteAsync
                (
                    stepSql,
                    new
                    {
                        RecipeId = recipeId,
                        StepNumber = i + 1,
                        Description = recipeRequest.Steps[i]
                    },
                    tran
                );
            }

            foreach (var meal in recipeRequest.Meals)
            {
                const string mealSql = @"
                        INSERT INTO recipe_meal_type (recipe_id, meal_type_id)
                        VALUES (@RecipeId, (SELECT id from reference.meal_type WHERE name = @MealType))
                    ";

                await conn.ExecuteAsync(mealSql, new { RecipeId = recipeId, MealType = meal }, tran);
            }

            foreach (var season in recipeRequest.Seasons)
            {
                const string seasonSql = @"
                        INSERT INTO recipe_season (recipe_id, season_id)
                        VALUES (@RecipeId, (SELECT id from reference.season WHERE name = @Season))
                    ";

                await conn.ExecuteAsync(seasonSql, new { RecipeId = recipeId, Season = season }, tran);
            }

            tran.Commit();
            return await GetByIdAsync(recipeId);
        }
        catch (Exception exc)
        {
            tran.Rollback();
            throw;
        }
        //_context.Recipes.Add(recipe);
        //await _context.SaveChangesAsync();
        //return recipe;
    }

    public async Task<Recipe> UpdateAsync(UpdateRecipeRequest recipeRequest)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        using var tran = conn.BeginTransaction();

        try
        {
            const string recipeSql = @"
                UPDATE recipe 
                SET
                    title = @Title,
                    description = @Description,
                    link = @Link,
                    cookbook = @Cookbook,
                    cookbook_image_url = @CookbookImageUrl,
                    recipe_image_url = @RecipeImageUrl,
                    is_favorite = @IsFavorite,
                    cooked = @Cooked,
                    date_cooked = @DateCooked,
                    chef = @Chef
                WHERE id = @Id
            ";

            var recipeId = await conn.ExecuteAsync(recipeSql, recipeRequest, tran);

            await conn.ExecuteAsync("DELETE FROM recipe_ingredient WHERE recipe_id = @RecipeId", new { RecipeId = recipeId });
            await conn.ExecuteAsync("DELETE FROM step WHERE recipe_id = @RecipeId", new { RecipeId = recipeId });
            await conn.ExecuteAsync("DELETE FROM recipe_meal_type WHERE recipe_id = @RecipeId", new { RecipeId = recipeId });
            await conn.ExecuteAsync("DELETE FROM recipe_season WHERE recipe_id = @RecipeId", new { RecipeId = recipeId });

            foreach (var ingredientDto in recipeRequest.Ingredients)
            {
                const string getIngredientsSql = @"
                    INSERT INTO ingredient (name)
                    Values (@Name) 
                    ON CONFLICT(NAME) DO UPDATE SET name = EXCLUDED.name
                    RETURNING id
                ";

                var ingredientId = await conn.ExecuteScalarAsync<int>
                (
                    getIngredientsSql,
                    new { Name = ingredientDto.IngredientName },
                    tran
                );

                const string linkSql = @"
                    INSERT INTO recipe_ingredient (recipe_id, ingredient_id, amount, unit, is_optional, note)
                    VALUES (@RecipeId, @IngredientId, @Amount, @Unit, @IsOptional, @Note)
                ";

                await conn.ExecuteAsync
                (
                    linkSql,
                    new
                    {
                        RecipeId = recipeId,
                        IngredientId = ingredientId,
                        ingredientDto.Amount,
                        ingredientDto.Unit,
                        ingredientDto.IsOptional,
                        ingredientDto.Note
                    },
                    tran
                );
            }

            for (int i = 0; i < recipeRequest.Steps.Count; i++)
            {
                const string stepSql = @"
                        INSERT INTO step (recipe_id, step_number, description)
                        VALUES (@RecipeId, @StepNumber, @Description)
                    ";

                await conn.ExecuteAsync
                (
                    stepSql,
                    new
                    {
                        RecipeId = recipeId,
                        StepNumber = i + 1,
                        Description = recipeRequest.Steps[i]
                    },
                    tran
                );
            }

            foreach (var meal in recipeRequest.Meals)
            {
                const string mealSql = @"
                        INSERT INTO recipe_meal_type (recipe_id, meal_type_id)
                        VALUES (@RecipeId, (SELECT id from reference.meal_type WHERE name = @MealType))
                    ";

                await conn.ExecuteAsync(mealSql, new { RecipeId = recipeId, MealType = meal }, tran);
            }

            foreach (var season in recipeRequest.Seasons)
            {
                const string seasonSql = @"
                        INSERT INTO recipe_season (recipe_id, season_id)
                        VALUES (@RecipeId, (SELECT id from reference.season WHERE name = @Season))
                    ";

                await conn.ExecuteAsync(seasonSql, new { RecipeId = recipeId, Season = season }, tran);
            }

            tran.Commit();
            return await GetByIdAsync(recipeId);
        }
        catch (Exception exc)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = _context.GetConnection();
        conn.Open();
        using var tran = conn.BeginTransaction();

        try
        {
            await conn.ExecuteAsync("DELETE FROM recipe_ingredient WHERE recipe_id = @RecipeId", new { RecipeId = id });
            await conn.ExecuteAsync("DELETE FROM step WHERE recipe_id = @RecipeId", new { RecipeId = id });
            await conn.ExecuteAsync("DELETE FROM recipe_meal_type WHERE recipe_id = @RecipeId", new { RecipeId = id });
            await conn.ExecuteAsync("DELETE FROM recipe_season WHERE recipe_id = @RecipeId", new { RecipeId = id });

            var result = await conn.ExecuteAsync("DELETE FROM recipe WHERE id = @RecipeId", new { RecipeId = id });
            tran.Commit();
            return result > 0;
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }
}
