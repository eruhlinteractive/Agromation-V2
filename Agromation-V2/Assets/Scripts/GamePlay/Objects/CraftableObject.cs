using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableObject : Item
{
	[SerializeField] private int[,] craftingRecipe;

	public int[,] Recipe { get { return craftingRecipe; } }
}
