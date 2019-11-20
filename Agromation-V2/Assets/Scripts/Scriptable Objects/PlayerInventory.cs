using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
	/// <summary>
	/// Purpose: Be a holder for the players inventory
	/// </summary>


	private static PlayerInventory instance;
	public static PlayerInventory Instance { get { return instance; } }

	[SerializeField] private ItemManager _itemManager;
	
	[SerializeField]private int maxItemsInInventory = 5;
	private Dictionary<int, int> itemsInInventory = new Dictionary<int, int>();


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		
	}
	private void OnEnable()
	{
		//_itemManager = GameSettings.Instance.ItemManager;
	}


	/// <summary>
	/// Checks to see if an item is in the players inventory
	/// </summary>
	/// <param name="itemId">The id of the item to look for</param>
	/// <returns>True if the item is in the inventory</returns>
	public bool IsItemInInventory(int itemId)
	{
		if (itemsInInventory.ContainsKey(itemId))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Finds how many of a specific object are in the players inventory
	/// </summary>
	/// <param name="itemId">The id of the item to check</param>
	/// <returns>The amount of the object in the inventory</returns>
	public int AmountInInventory(int itemId)
	{
		if (itemsInInventory.ContainsKey(itemId))
		{
			return itemsInInventory[itemId];
		}
		else
		{
			return 0;
		}
	}

	/// <summary>
	/// Adds an item of a specified ID to the inventory
	/// </summary>
	/// <param name="itemId">The id of the item to add</param>
	public bool AddToInventory(int itemId)
	{ 

		//Is there room in the inventory?
		if (itemsInInventory.Count <= maxItemsInInventory)
		{
			//Is it a valid item?
			if (_itemManager.ValidItem(itemId))
			{
				Debug.Log("Added item: " + itemId + " to inventory");
				itemsInInventory.Add(itemId,1);
				return true;
			}
		}
		return false;
	}

	public void RemoveFromInventory(int itemId)
	{
		
		//Is the itemId valid?
		if (_itemManager.ValidItem(itemId))
		{
			//Is it in the inventory already?
			if (itemsInInventory.ContainsKey(itemId))
			{
				itemsInInventory[itemId]--;

				//If there arent any left in the inventory
				if(itemsInInventory[itemId] <= 0)
				{
					itemsInInventory.Remove(itemId);
				}
			}
		}
	}

}
