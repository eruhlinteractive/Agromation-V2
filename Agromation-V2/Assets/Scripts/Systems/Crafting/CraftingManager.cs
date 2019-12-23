using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
	//Fields
	[SerializeField] List<int> craftableItems;
	[SerializeField] private ItemManager _itemManager;
	[SerializeField] private PlayerControlManager _playerControlManager;
	[SerializeField] private GameObject craftingMenu;
	private PlayerInventory _playerInv;
	[SerializeField] private Transform objectSpawnPosition;


	//Properties
	public List<int> CraftableItems { get => craftableItems; }
	// Start is called before the first frame update
	void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		_playerControlManager = GameSettings.Instance.PlayerControlManager;
		_playerInv = GameSettings.Instance.PlayerInventory;
		if(objectSpawnPosition == null)
		{
			throw new System.Exception("CRAFTING MANAGER SPAWN POSITION NOT SET");
		}

		craftingMenu = UIResources.Instance.CraftingUI;
		//craftingMenu.SetActive(false);
	}

	/// <summary>
	/// Opens crafing menu and locks player movement
	/// </summary>
	public void OpenCraftingMenu()
	{
		_playerControlManager.UnlockCursor();
		craftingMenu.SetActive(true);
		craftingMenu.GetComponent<UICraftingMenu>().MenuOpened(this);
	}

	/// <summary>
	/// Closes the crafting menu Ui and unlocks player movement
	/// </summary>
	public void CloseCraftingMenu()
	{
		_playerControlManager.LockCursor();
		craftingMenu.SetActive(false);
	}

	/// <summary>
	/// Try to craft an item
	/// </summary>
	/// <param name="itemId">The id of the object to craft</param>
	/// <returns>TRUE if the object was successfully crafted</returns>
	public bool TryCrafting(int itemId)
	{
		CraftableObject objectToCraft = _itemManager.GetItem(itemId).GetComponent<CraftableObject>();

		//The player has enough of each item to craft the object
		if(CheckInventoryForIngredients(objectToCraft))
		{
			//Debug.Log("Crafting Sequence Initiated....");
			CraftObject(objectToCraft);
			//Remove Required materials from the players inventory
		
			return true;
		}
		return false;

	}

	/// <summary>
	/// Checks if player has enough of each ingredient to craft the item
	/// </summary>
	/// <param name="objToCraft">The craftableObject scripy of the object to craft</param>
	/// <returns>TRUE if the player has all required ingredients</returns>
	private bool CheckInventoryForIngredients(CraftableObject objToCraft)
	{
		//Loop through each of the objects ingredients
		for (int i = 0; i < objToCraft.Ingredients.Count; i++)
		{
			//Is the ingredient in the players inventory
			if (_playerInv.IsItemInInventory(objToCraft.Ingredients[i]))
			{
				int requiredIngredientAmount = objToCraft.GetIngredientAmount(objToCraft.Ingredients[i]);

				if(requiredIngredientAmount != -1)
				{
					//If the player DOES NOT have enough of the ingredient to craft the item
					if (requiredIngredientAmount > _playerInv.AmountInInventory(objToCraft.Ingredients[i]))
					{
						//Debug.Log("NOT Enough" + objToCraft.Ingredients[i] + " In inventory");
						return false;
					}
				}
				else
				{
					//Debug.Log(objToCraft.Ingredients[i] + "Not part of recipe");
					return false;
				}
			}
			else
			{
				//Debug.Log("Not In inventory");
				//Break out and stop checking 
				return false;
			}
		}
		//Debug.Log("All ingredients required!");
		return true;
	}

	/// <summary>
	/// Called when an object can be successfully crafted
	/// </summary>
	/// <param name="id">The id of the item to craft</param>
	/// <param name="objectToCraft">The CraftableObject component of the object to craft</param>
private void CraftObject(CraftableObject objectToCraft)
{
	for (int i = 0; i < objectToCraft.Ingredients.Count; i++)
	{
		//Debug.Log(objectToCraft.Ingredients[i]);
		int ingredient = objectToCraft.Ingredients[i];

		//Remove the material from the inventory
		_playerInv.RemoveFromInventory(ingredient, objectToCraft.GetIngredientAmount(ingredient));
	}


	//Try to add the item directly to inventory
	if (!_playerInv.AddToInventory(objectToCraft.Id))
	{
		Debug.Log(objectToCraft.Id);
		//If that fails, spawn the item instead
		Instantiate(_itemManager.GetItem(objectToCraft.Id), objectSpawnPosition.position, Quaternion.identity);
	}
		
}

	//TODO: Fix this XD
//#if UNITY_EDITOR
//	private void OnValidate()
//	{
//		if(_itemManager != null)
//		{
//			//Make sure each item is craftable
//			for (int i = 0; i < craftableItems.Count; i++)
//			{
//				if(_itemManager.GetItem(craftableItems[i]).GetComponent<CraftableObject>() == null)
//				{
//					craftableItems.RemoveAt(i);
//					i--;
//				}
//			}
//		}
//	}
//#endif
}
