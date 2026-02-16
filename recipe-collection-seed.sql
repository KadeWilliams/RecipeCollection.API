DROP TABLE IF EXISTS recipe_season;
DROP TABLE IF EXISTS recipe_meal_type;
DROP TABLE IF EXISTS reference.season;
DROP TABLE IF EXISTS reference.meal_type;
DROP TABLE IF EXISTS recipe_ingredient;
DROP TABLE IF EXISTS ingredient;
DROP TABLE IF EXISTS step;
DROP TABLE IF EXISTS recipe;

DROP SCHEMA reference;

CREATE SCHEMA reference;

CREATE TABLE IF NOT EXISTS reference.season (
    id SERIAL PRIMARY KEY,
    name VARCHAR(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS reference.meal_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS ingredient (
    id serial primary key,
    name varchar(255) not null unique
);

CREATE TABLE IF NOT EXISTS recipe (
  id SERIAL PRIMARY KEY,
  title VARCHAR(255) NOT NULL,
  description TEXT NOT NULL,
  link TEXT NULL, 
  cookbook VARCHAR(255) NULL,
  cookbook_image_url TEXT NULL,
  recipe_image_url TEXT NULL, 
  is_favorite BOOLEAN DEFAULT FALSE not null,
  cooked BOOLEAN DEFAULT FALSE not null, 
  date_cooked date null,  
  chef varchar(255) not null,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS step (
    id serial primary key,
    recipe_id int not null references recipe(id),
    step_number int not null, 
    description text not null,
    unique (recipe_id, step_number)
);

CREATE TABLE IF NOT EXISTS recipe_meal_type(
    recipe_id int not null references recipe(id),
    meal_type_id int not null references reference.meal_type(id),
    PRIMARY KEY (recipe_id, meal_type_id)
);

CREATE TABLE IF NOT EXISTS recipe_season (
    recipe_id int not null references recipe(id),
    season_id int not null references reference.season(id),
    PRIMARY KEY (recipe_id, season_id)
);

CREATE TABLE IF NOT EXISTS recipe_ingredient (
    recipe_id int not null references recipe(id), 
    ingredient_id int not null references ingredient(id),
    amount varchar(50) not null,
    unit varchar(50) null, 
    is_optional boolean default TRUE not null, 
    note text null,
    PRIMARY KEY (recipe_id, ingredient_id)
);

-- Insert reference data
INSERT INTO reference.season (name) VALUES
('spring'),
('summer'),
('fall'),
('winter');

INSERT INTO reference.meal_type (name) VALUES
('breakfast'),
('lunch'),
('dinner'),
('snack');

-- Insert ingredients
INSERT INTO ingredient (name) VALUES
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
    title, 
    description, 
    link, 
    cookbook, 
    cookbook_image_url, 
    recipe_image_url, 
    is_favorite, 
    cooked, 
    date_cooked, 
    chef
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
INSERT INTO recipe_meal_type (recipe_id, meal_type_id) VALUES
(1, (SELECT id FROM reference.meal_type WHERE name = 'lunch')),
(1, (SELECT id FROM reference.meal_type WHERE name = 'dinner'));

-- Insert recipe-season relationships
INSERT INTO recipe_season (recipe_id, season_id) VALUES
(1, (SELECT id FROM reference.season WHERE name = 'spring')),
(1, (SELECT id FROM reference.season WHERE name = 'summer')),
(1, (SELECT id FROM reference.season WHERE name = 'fall')),
(1, (SELECT id FROM reference.season WHERE name = 'winter'));

-- Insert recipe ingredients
INSERT INTO recipe_ingredient (recipe_id, ingredient_id, amount, unit, is_optional, note) VALUES
(1, (SELECT id FROM ingredient WHERE name = 'salmon'), '24', 'ounces', FALSE, NULL),
(1, (SELECT id FROM ingredient WHERE name = 'salt'), 'taste', NULL, FALSE, NULL),
(1, (SELECT id FROM ingredient WHERE name = 'pepper'), 'taste', NULL, FALSE, NULL),
(1, (SELECT id FROM ingredient WHERE name = 'greek yogurt'), '60', 'grams', FALSE, NULL),
(1, (SELECT id FROM ingredient WHERE name = 'harissa paste'), '35', 'grams', TRUE, 'Couldn''t really taste it; felt like it didn''t need it.'),
(1, (SELECT id FROM ingredient WHERE name = 'bread crumbs'), '80', 'grams', FALSE, NULL),
(1, (SELECT id FROM ingredient WHERE name = 'flaky salt'), '80', 'grams', TRUE, NULL),
(1, (SELECT id FROM ingredient WHERE name = 'spray oil'), 'as needed', NULL, FALSE, NULL);

-- Insert steps
INSERT INTO step (recipe_id, step_number, description) VALUES
(1, 1, 'Position a rack to the center of the oven and preheat to 425 F convection.'),
(1, 2, 'Season both sides of the salmon with kosher salt and black pepper.'),
(1, 3, 'Slice the salmon into 1-inch cubes.'),
(1, 4, 'Add the salmon pieces to a large bowl, add in 1/4 cup of greek yogurt and 2 tablespoons mild harissa paste. Mix well so that the salmon is fully coated. See Note.'),
(1, 5, 'In a small dish to the side, add the 2/3 cup of almond flour and season with 1/2 teaspoon flaky salt. Toss to break up any clumps.'),
(1, 6, 'Prepare a parchment-lined baking sheet to the side. Take each salmon nugget and lightly toss it in the almond flour to get it coated on all sides. Add these to the baking sheet. If your salmon has skin on it, make sure you place these skin-side-down.'),
(1, 7, 'Lightly spray the nuggets with spray avocado oil. This is optional, but will help them get some color.'),
(1, 8, 'Bake at 425 F for 20-22 minutes or until golden and crisp on the bottoms.'),
(1, 9, 'Serve warm!');

