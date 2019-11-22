using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
	#region Fields
	[SerializeField] private InvSlot[] inventorySlots = new InvSlot[5];
	private Dictionary<int, InvSlot> _slots = new Dictionary<int, InvSlot>(); // Format <item Id, Inventory slot displaying data>
	[SerializeField] private int selectedSlot = 0;
	[SerializeField] private PlayerInventory _playerInventory = null;
	[SerializeField] private ItemManager _itemManager = null;


	#endregion

	#region Methods
	// Start is called before the first frame update
	void Start()
	{
		//Set playerInventory Reference
		_playerInventory = GameSettings.Instance.PlayerInventory;
		_itemManager = GameSettings.Instance.ItemManager;

		//Bind Delegates
		PlayerInventory.addedItem += BindSlot;
		PlayerInventory.removedItem += UnBindSlot;
		PlayerInventory.itemAmountUpdate += UpdateItemAmount;

		//populate inventorySlots dictionary
		for (int i = 0; i < inventorySlots.Length; i++)
		{
			inventorySlots[i] = transform.GetChild(i).GetComponent<InvSlot>();
		}
	}

	private void Update()
	{
		SetCurrentSlot();
		_playerInventory.currentSelectedId = inventorySlots[selectedSlot].Id;
	}

	/// <summary>
	/// Determines which slot is selected
	/// </summary>
	private void SetCurrentSlot()
	{
		inventorySlots[selectedSlot].gameObject.transform.localScale = Vector3.one;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			selectedSlot = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			selectedSlot = 1;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			selectedSlot = 2;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			selectedSlot = 3;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			selectedSlot = 4;
		}

		//Set which object should be displayed in the hand
		HandObject.Instance.SetCurrentItem(inventorySlots[selectedSlot].Id);
		inventorySlots[selectedSlot].gameObject.transform.localScale = Vector3.one * 1.15f;
	}


	/// <summary>
	/// Finds a cleared, open slot 
	/// </summary>
	/// <returns>An open slot, if there are any</returns>
	private InvSlot FindOpenSlot()
	{
		for (int i = 0; i < inventorySlots.Length; i++)
		{
			if(inventorySlots[i].Id == -1)
			{
				return inventorySlots[i];
			}
		}
		return null;

	}


	/// <summary>
	/// Runs when an EMPTY slot is being bound to an id
	/// </summary>
	/// <param name="itemIdToBind">The id of the item in this slot</param>
	private void BindSlot(int itemIdToBind)
	{
		//First, find an open slot
		InvSlot openSlot = FindOpenSlot();

		//Once a slot is found, bind the data
		if(openSlot != null)
		{
			openSlot.SetData(itemIdToBind, 1, _itemManager.GetItem(itemIdToBind).GetComponent<Item>().Icon);
			_slots.Add(itemIdToBind, openSlot);
		}
		
	}

	/// <summary>
	/// Clears a slot (purgeData) and prepares it to be rebound
	/// </summary>
	/// <param name="itemIdToUnbind">The id of the item currently in the slot</param>
	private void UnBindSlot(int itemIdToUnbind)
	{
		if (_slots.ContainsKey(itemIdToUnbind))
		{
			//Clear Slot
			_slots[itemIdToUnbind].PurgeData();
			
			//Unbind the slot
			_slots.Remove(itemIdToUnbind);
		}
	}

	/// <summary>
	/// Updates the amount value if the slot bound to the itemId
	/// </summary>
	/// <param name="itemId">The id of the item to update</param>
	/// <param name="newAmount">The new amount of the item in the inventory</param>
	private void UpdateItemAmount(int itemId, int newAmount) 
	{
		if (_slots.ContainsKey(itemId))
		{
			_slots[itemId].SetAmount(newAmount);
		}
		
	} 
	#endregion
}
