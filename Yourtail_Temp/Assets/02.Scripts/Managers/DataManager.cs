using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public Dictionary<string, Cocktail> RecipeCollection { get; private set; } = new Dictionary<string, Cocktail>();

    public void AddRecipe(Cocktail cocktail)
	{
		string key = cocktail.Id;

		if (RecipeCollection.ContainsKey(key))
			return;

		RecipeCollection.Add(key, cocktail);
	}
}
