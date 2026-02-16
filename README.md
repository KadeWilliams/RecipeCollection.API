Test structures for each request verb.

POST: 
Content-Type: application/json
```json
{
    "title": "Test Recipe",
    "description": "Something to test",
    "link": null,
    "cookbook": null,
    "cookbookImageUrl": null,
    "recipeImageUrl": null,
    "isFavorite": false,
    "cooked": true,
    "dateCooked": "2026-02-14",
    "chef": "Kade Williams",
    "meals": [
        "dinner",
        "lunch",
        "breakfast"
    ],
    "seasons": [
        "winter",
        "fall",
        "summer",
        "spring"
    ],
    "ingredients": [
        {
            "ingredientName": "potatoes",
            "amount": "5",
            "unit": "pounds",
            "isOptional": false,
            "note": "Couldn't live without potatoes"
        }
    ],
    "steps": [
        "Peel",
        "Serve!"
    ]
}
```