using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingManager : MonoBehaviour
{
	[SerializeField] private PlayerStats _playerStats;
	[SerializeField] private ItemManager _itemManager;
	[SerializeField] private Transform itemSpawnPoint;
	[SerializeField] private PlayerControlManager _playerControlManager;
	[SerializeField] private GameObject buyMenu;
	private PlayerInventory _playerInv;

	public delegate void ItemBought(bool wasItemBought);
	public static ItemBought wasItemBought;

	// Start is called before the first frame update
	void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		_playerStats = GameSettings.Instance.PlayerStats;
		_playerControlManager = GameSettings.Instance.PlayerControlManager;
		_playerInv = GameSettings.Instance.PlayerInventory;
		buyMenu.SetActive(false);

		UIBuyingMenuController.tryPurchase += BuyItem;
    }

	/// <summary>
	/// Opens buying menu and locks player movement
	/// </summary>
	public void OpenBuyingMenu()
	{
		_playerControlManager.UnlockCursor();
		buyMenu.SetActive(true);
	}

	/// <summary>
	/// Closes the buying menu Ui and unlocks player movement
	/// </summary>
	public void CloseBuyingMenu()
	{
		_playerControlManager.LockCursor();
		buyMenu.SetActive(false);
	}

	/// <summary>
	/// Buy the currently selected item from the menu
	/// </summary>
	/// <param name="itemId">The itemId of the item being bought</param>
	public void BuyItem(int itemId)
	{
		int itemValue = _itemManager.GetItem(itemId).gameObject.GetComponent<Item>().Price;

		//Check if the player can buy the item
		if(_playerStats.Money >= itemValue)
		{
			_playerStats.RemoveMoney(itemValue);
			{
				//Try to add the item directly to inventory
				if (!_playerInv.AddToInventory(itemId))
				{
					Debug.Log(itemId);
					//If that fails, spawn the item instead
					SpawnItem(itemId);
				}
			}
			//Send feedBack to the UI
			wasItemBought(true);
		}
		else
		{
			//Send feedBack to the UI
			wasItemBought(false);
		}
	}

	/// <summary>
	/// Spawns the bought Item
	/// </summary>
	/// <param name="itemId"></param>
	private void SpawnItem(int itemId)
	{
		if(_itemManager.ValidItem(itemId))
		{
			GameObject itemToSpawn = _itemManager.GetItem(itemId);
			Instantiate(itemToSpawn, itemSpawnPoint.position, Quaternion.identity);
		}
		
	}
}
