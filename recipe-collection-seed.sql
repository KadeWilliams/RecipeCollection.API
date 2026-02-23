DROP TABLE IF EXISTS recipe_season CASCADE;
DROP TABLE IF EXISTS recipe_meal_type CASCADE;
DROP TABLE IF EXISTS reference.season CASCADE;
DROP TABLE IF EXISTS reference.meal_type CASCADE;
DROP TABLE IF EXISTS recipe_ingredient CASCADE;
DROP TABLE IF EXISTS ingredient CASCADE;
DROP TABLE IF EXISTS step CASCADE;
DROP TABLE IF EXISTS recipe CASCADE;

DROP SCHEMA reference;

CREATE SCHEMA reference;

CREATE TABLE IF NOT EXISTS reference.season (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS reference.meal_type (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS ingredient (
    Id serial primary key,
    Name varchar(255) not null unique
);

CREATE TABLE IF NOT EXISTS recipe (
  Id SERIAL PRIMARY KEY,
  Title VARCHAR(255) NOT NULL,
  Description TEXT NOT NULL,
  Link TEXT NULL, 
  Cookbook VARCHAR(255) NULL,
  CookbookImageUrl TEXT NULL,
  RecipeImageUrl TEXT NULL, 
  IsFavorite BOOLEAN DEFAULT FALSE not null,
  Cooked BOOLEAN DEFAULT FALSE not null, 
  DateCooked date null,  
  Chef varchar(255) not null,
  CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS step (
    Id serial primary key,
    RecipeId int not null references recipe(Id),
    StepNumber int not null, 
    Description text not null,
    unique (RecipeId, StepNumber)
);

CREATE TABLE IF NOT EXISTS recipe_meal_type(
    RecipeId int not null references recipe(Id),
    MealTypeId int not null references reference.meal_type(Id),
    PRIMARY KEY (RecipeId, MealTypeId)
);

CREATE TABLE IF NOT EXISTS recipe_season (
    RecipeId int not null references recipe(Id),
    SeasonId int not null references reference.season(Id),
    PRIMARY KEY (RecipeId, SeasonId)
);

CREATE TABLE IF NOT EXISTS recipe_ingredient (
    RecipeId int not null references recipe(Id), 
    IngredientId int not null references ingredient(Id),
    Amount varchar(50) not null,
    Unit varchar(50) null, 
    IsOptional boolean default TRUE not null, 
    Note text null,
    PRIMARY KEY (RecipeId, IngredientId)
);

-- Insert reference data
INSERT INTO reference.season (Name) VALUES
('spring'),
('summer'),
('fall'),
('winter');

INSERT INTO reference.meal_type (Name) VALUES
('breakfast'),
('lunch'),
('dinner'),
('snack');

-- Insert ingredients
INSERT INTO ingredient (Name) VALUES
('salmon'),
('salt'),
('pepper'),
('greek yogurt'),
('harissa paste'),
('bread crumbs'),
('flaky salt'),
('spray oil');

-- Insert recipe
INSERT INTO recipe (
    Title, 
    Description, 
    Link, 
    Cookbook, 
    CookbookImageUrl, 
    RecipeImageUrl, 
    IsFavorite, 
    cooked, 
    DateCooked, 
    Chef
) VALUES (
    'Salmon Nuggets',
    'These four-ingredient salmon nuggets are crispy, gluten-free, filled with flavor and so healthy. They are like my miracle nugget, and especially great if you are pescatarian but just craving a *hit* of fast-food flavor. C''mon, we all need it every now and then! These salmon nuggets are easy to make, they take minutes to assemble and then all they need to do is get all crispy in the oven. I love making these with wild sockeye salmon to keep them extra healthy, but they will work with any variety of salmon you have access to!',
    'https://justinesnacks.com/four-ingredient-salmon-nuggets/',
    NULL,
    NULL,
    'https://i0.wp.com/justinesnacks.com/wp-content/uploads/2025/02/crispy-healthy-salmon-nuggets.jpg?resize=768%2C960&ssl=1',
    TRUE,
    TRUE,
    '2026-01-14',
    'Justine Doiron'
);

-- Insert recipe-meal type relationships (assuming recipe_id = 1)
INSERT INTO recipe_meal_type (RecipeId, MealTypeId) VALUES
(1, (SELECT Id FROM reference.meal_type WHERE Name = 'lunch')),
(1, (SELECT Id FROM reference.meal_type WHERE Name = 'dinner'));

-- Insert recipe-season relationships
INSERT INTO recipe_season (RecipeId, SeasonId) VALUES
(1, (SELECT Id FROM reference.season WHERE Name = 'spring')),
(1, (SELECT Id FROM reference.season WHERE Name = 'summer')),
(1, (SELECT Id FROM reference.season WHERE Name = 'fall')),
(1, (SELECT Id FROM reference.season WHERE Name = 'winter'));

-- Insert recipe ingredients
INSERT INTO recipe_ingredient (RecipeId, IngredientId, Amount, Unit, IsOptional, Note) VALUES
(1, (SELECT Id FROM Ingredient WHERE Name = 'salmon'), '24', 'ounces', FALSE, NULL),
(1, (SELECT Id FROM Ingredient WHERE Name = 'salt'), 'taste', NULL, FALSE, NULL),
(1, (SELECT Id FROM Ingredient WHERE Name = 'pepper'), 'taste', NULL, FALSE, NULL),
(1, (SELECT Id FROM Ingredient WHERE Name = 'greek yogurt'), '60', 'grams', FALSE, NULL),
(1, (SELECT Id FROM Ingredient WHERE Name = 'harissa paste'), '35', 'grams', TRUE, 'Couldn''t really taste it; felt like it didn''t need it.'),
(1, (SELECT Id FROM Ingredient WHERE Name = 'bread crumbs'), '80', 'grams', FALSE, NULL),
(1, (SELECT Id FROM Ingredient WHERE Name = 'flaky salt'), '80', 'grams', TRUE, NULL),
(1, (SELECT Id FROM Ingredient WHERE Name = 'spray oil'), 'as needed', NULL, FALSE, NULL);

-- Insert steps
INSERT INTO step (RecipeId, StepNumber, Description) VALUES
(1, 1, 'Position a rack to the center of the oven and preheat to 425 F convection.'),
(1, 2, 'Season both sides of the salmon with kosher salt and black pepper.'),
(1, 3, 'Slice the salmon into 1-inch cubes.'),
(1, 4, 'Add the salmon pieces to a large bowl, add in 1/4 cup of greek yogurt and 2 tablespoons mild harissa paste. Mix well so that the salmon is fully coated. See Note.'),
(1, 5, 'In a small dish to the side, add the 2/3 cup of almond flour and season with 1/2 teaspoon flaky salt. Toss to break up any clumps.'),
(1, 6, 'Prepare a parchment-lined baking sheet to the side. Take each salmon nugget and lightly toss it in the almond flour to get it coated on all sides. Add these to the baking sheet. If your salmon has skin on it, make sure you place these skin-side-down.'),
(1, 7, 'Lightly spray the nuggets with spray avocado oil. This is optional, but will help them get some color.'),
(1, 8, 'Bake at 425 F for 20-22 minutes or until golden and crisp on the bottoms.'),
(1, 9, 'Serve warm!');

INSERT INTO recipe (
    Title, 
    Description, 
    Link, 
    Cookbook, 
    CookbookImageUrl, 
    RecipeImageUrl, 
    IsFavorite, 
    cooked, 
    DateCooked, 
    Chef
) VALUES (
    'Salmon Nuggets',
    'These four-ingredient salmon nuggets are crispy, gluten-free, filled with flavor and so healthy. They are like my miracle nugget, and especially great if you are pescatarian but just craving a *hit* of fast-food flavor. C''mon, we all need it every now and then! These salmon nuggets are easy to make, they take minutes to assemble and then all they need to do is get all crispy in the oven. I love making these with wild sockeye salmon to keep them extra healthy, but they will work with any variety of salmon you have access to!',
    'https://justinesnacks.com/four-ingredient-salmon-nuggets/',
    NULL,
    NULL,
    'https://i0.wp.com/justinesnacks.com/wp-content/uploads/2025/02/crispy-healthy-salmon-nuggets.jpg?resize=768%2C960&ssl=1',
    TRUE,
    TRUE,
    '2026-01-14',
    'Justine Doiron'
);

-- Insert recipe-meal type relationships (assuming recipe_id = 1)
INSERT INTO recipe_meal_type (RecipeId, MealTypeId) VALUES
(2, (SELECT Id FROM reference.meal_type WHERE Name = 'lunch')),
(2, (SELECT Id FROM reference.meal_type WHERE Name = 'dinner'));

-- Insert recipe-season relationships
INSERT INTO recipe_season (RecipeId, SeasonId) VALUES
(2, (SELECT Id FROM reference.season WHERE Name = 'spring')),
(2, (SELECT Id FROM reference.season WHERE Name = 'summer')),
(2, (SELECT Id FROM reference.season WHERE Name = 'fall')),
(2, (SELECT Id FROM reference.season WHERE Name = 'winter'));

-- Insert recipe ingredients
INSERT INTO recipe_ingredient (RecipeId, IngredientId, Amount, Unit, IsOptional, Note) VALUES
(2, (SELECT Id FROM Ingredient WHERE Name = 'salmon'), '24', 'ounces', FALSE, NULL),
(2, (SELECT Id FROM Ingredient WHERE Name = 'salt'), 'taste', NULL, FALSE, NULL),
(2, (SELECT Id FROM Ingredient WHERE Name = 'pepper'), 'taste', NULL, FALSE, NULL),
(2, (SELECT Id FROM Ingredient WHERE Name = 'greek yogurt'), '60', 'grams', FALSE, NULL),
(2, (SELECT Id FROM Ingredient WHERE Name = 'harissa paste'), '35', 'grams', TRUE, 'Couldn''t really taste it; felt like it didn''t need it.'),
(2, (SELECT Id FROM Ingredient WHERE Name = 'bread crumbs'), '80', 'grams', FALSE, NULL),
(2, (SELECT Id FROM Ingredient WHERE Name = 'flaky salt'), '80', 'grams', TRUE, NULL),
(2, (SELECT Id FROM Ingredient WHERE Name = 'spray oil'), 'as needed', NULL, FALSE, NULL);

-- Insert steps
INSERT INTO step (RecipeId, StepNumber, Description) VALUES
(2, 1, 'Position a rack to the center of the oven and preheat to 425 F convection.'),
(2, 2, 'Season both sides of the salmon with kosher salt and black pepper.'),
(2, 3, 'Slice the salmon into 1-inch cubes.'),
(2, 4, 'Add the salmon pieces to a large bowl, add in 1/4 cup of greek yogurt and 2 tablespoons mild harissa paste. Mix well so that the salmon is fully coated. See Note.'),
(2, 5, 'In a small dish to the side, add the 2/3 cup of almond flour and season with 1/2 teaspoon flaky salt. Toss to break up any clumps.'),
(2, 6, 'Prepare a parchment-lined baking sheet to the side. Take each salmon nugget and lightly toss it in the almond flour to get it coated on all sides. Add these to the baking sheet. If your salmon has skin on it, make sure you place these skin-side-down.'),
(2, 7, 'Lightly spray the nuggets with spray avocado oil. This is optional, but will help them get some color.'),
(2, 8, 'Bake at 425 F for 20-22 minutes or until golden and crisp on the bottoms.'),
(2, 9, 'Serve warm!');
