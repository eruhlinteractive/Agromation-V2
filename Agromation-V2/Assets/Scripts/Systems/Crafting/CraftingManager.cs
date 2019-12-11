using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
	//Fields
	[SerializeField] List<int> craftableItems;
	[SerializeField] private ItemManager _itemManager;
	[SerializeField] private Transform itemSpawnPoint;
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
	public void CloseBuyingMenu()
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
			CraftObject(itemId,objectToCraft);
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
		foreach (int ingredientId in objToCraft.Recipe.Keys)
		{
			//Is the ingredient in the players inventory
			if (_playerInv.AmountInInventory(ingredientId) != 0)
			{
				//If the player DOES NOT have enough of the ingredient to craft the item
				if (_playerInv.AmountInInventory(ingredientId) < objToCraft.Recipe[ingredientId])
				{
					//Break out and stop checking 
					return false;
				}
			}
			else
			{
				//Break out and stop checking 
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Called when an object can be successfully crafted
	/// </summary>
	/// <param name="id">The id of the item to craft</param>
	/// <param name="objectToCraft">The CraftableObject component of the object to craft</param>
	private void CraftObject(int id, CraftableObject objectToCraft)
	{
		//Remove Required materials from the players inventory
		foreach (int ingredientId in objectToCraft.Recipe.Keys)
		{
			_playerInv.RemoveFromInventory(ingredientId, objectToCraft.Recipe[ingredientId]);
		}
			//Try to add the item directly to inventory
		if (!_playerInv.AddToInventory(id))
		{
			//Debug.Log(itemId);
			//If that fails, spawn the item instead
			Instantiate(_itemManager.GetItem(id), objectSpawnPosition.position, Quaternion.identity);
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
