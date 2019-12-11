using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableObject : Item
{
	[SerializeField] private Dictionary<int, int> craftingRecipe = new Dictionary<int, int>();

	public Dictionary<int,int> Recipe { get { return craftingRecipe; } }
	[Header("Crafting Recipe")]
	[SerializeField] List<int> _ingredients = new List<int>();
	[SerializeField] List<int> _ingredientAmounts = new List<int>();

//TODO: Fix This
	private void OnValidate()
	{
		//Populate dictionary
		if(_ingredients.Count > 0 && _ingredientAmounts.Count > 0 && _ingredients.Count == _ingredientAmounts.Count)
		{
			if(craftingRecipe != null)
			{
				craftingRecipe = new Dictionary<int, int>();
				for (int i = 0; i < _ingredients.Count; i++)
				{
					craftingRecipe.Add(_ingredients[i], _ingredientAmounts[i]);
				}
			}
		}
	}
}
