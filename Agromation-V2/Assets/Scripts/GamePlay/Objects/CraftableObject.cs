using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableObject : Item
{
	[SerializeField] private Dictionary<int, int> craftingRecipe = new Dictionary<int, int>();

	public Dictionary<int,int> Recipe { get { return craftingRecipe; } }

	public List<int> Ingredients { get => _ingredients; }

	[Header("Crafting Recipe")]
	[SerializeField] List<int> _ingredients = new List<int>();
	[SerializeField] List<int> _ingredientAmounts = new List<int>();

	private void Start()
	{
		for (int i = 0; i < _ingredients.Count; i++)
		{
			craftingRecipe.Add(_ingredients[i], _ingredientAmounts[i]);
		}


		if(craftingRecipe.Count == 0)
		{
			throw new System.Exception("Craftable Item Recipe is Empty!: " + this.ItemName);
		}
	}

	public int GetIngredientAmount(int ingredientToGet)
	{
		if (_ingredients.Contains(ingredientToGet))
		{
			return _ingredientAmounts[_ingredients.IndexOf(ingredientToGet)];
		}
		else
		{
			return -1;
		}
	}
	//TODO: Fix This
	private void OnValidate()
	{
		////Populate dictionary
		//if(_ingredients.Count > 0 && _ingredientAmounts.Count > 0 && _ingredients.Count == _ingredientAmounts.Count)
		//{
		//	if(craftingRecipe != null)
		//	{
		//		craftingRecipe = new Dictionary<int, int>();
		//		for (int i = 0; i < _ingredients.Count; i++)
		//		{
		//			craftingRecipe.Add(_ingredients[i], _ingredientAmounts[i]);
		//		}
		//	}
		//}

		if(_ingredients.Count != _ingredientAmounts.Count)
		{
			throw new System.Exception("Item " + this.ItemName + "'s ingredients and ingredient amounts do not match!");
		}
	}
}
