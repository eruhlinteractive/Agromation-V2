using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
	#region Fields
	/// <summary>
	/// Purpose: Be a holder for the players inventory
	/// </summary>


	private static PlayerInventory instance = null;
	public static PlayerInventory Instance { get { return instance; } }

	[SerializeField] private ItemManager _itemManager = null;
	
	[SerializeField]private int maxItemsInInventory = 5;
	private Dictionary<int, int> itemsInInventory = new Dictionary<int, int>();
	public int currentSelectedId;

	//Delegates
	public delegate void InventoryAction(int itemId);
	public static InventoryAction addedItem;
	public static InventoryAction removedItem;


	public delegate void UpdatedItemAmount(int itemId, int amount);
	public static UpdatedItemAmount itemAmountUpdate;

	#endregion

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		
	}

	/// <summary>
	/// Called by the gameSettingsManager to start the gameObject
	/// </summary>
	public void Initalize()
	{
		_itemManager = GameSettings.Instance.ItemManager;
	}


	/// <summary>
	/// Checks to see if an item is in the players inventory
	/// </summary>
	/// <param name="itemId">The id of the item to look for</param>
	/// <returns>True if the item is in the inventory</returns>
	private bool IsItemInInventory(int itemId)
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

		//Debug.Log(itemsInInventory.Count + " items in inventory");
		//Is it a valid item?
		if (_itemManager.ValidItem(itemId))
		{
			//Debug.Log("VALID ITEM");
			//Check if the item is already in the inventory
			if (IsItemInInventory(itemId))
			{
				//Increment the item amount
				itemsInInventory[itemId]++;


				if(itemsInInventory.ContainsKey(itemId))
				itemAmountUpdate(itemId, itemsInInventory[itemId]);//Call delegate
				 //Debug.Log("Increased amount in inventory");
				return true;	//Item amount was updated
			}

			//Otherwise, if the item is NOT currently in the inventory
			else
			{
			//Is there room in the inventory?
				if (itemsInInventory.Count < maxItemsInInventory)
				{
					//Add to inventory
					//Debug.Log("Added item: " + itemId + " to inventory");
					itemsInInventory.Add(itemId, 1);

					//Call itemAdded Delegate
					addedItem(itemId);
				return true;
				}
				//There was no room left in the inventory
				return false;
				
			}
			
		}
			
	
		return false;
	}

	/// <summary>
	/// Removes an item of a specified ID from the inventory(if its already in it)
	/// </summary>
	/// <param name="itemId">The id of the item to remove</param>
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
					
					//Call removedItem Delegate
					removedItem(itemId);
				}
				else
				{
					//Update Amount
					itemAmountUpdate(itemId, itemsInInventory[itemId]);
				}
			}
		}
	}

}
