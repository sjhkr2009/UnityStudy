using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public List<string> RecipeCollection { get; private set; } = new List<string>();

    public bool AddRecipe(Cocktail cocktail)
	{
		string id = cocktail.Id;

		if (cocktail.IsDefault || RecipeCollection.Contains(id))
			return false;

		RecipeCollection.Add(id);
		return true;
	}
}
