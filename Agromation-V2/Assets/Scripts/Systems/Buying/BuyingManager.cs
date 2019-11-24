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

	public delegate void ItemBought(bool wasItemBought);
	public static ItemBought wasItemBought;

    // Start is called before the first frame update
    void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		_playerStats = GameSettings.Instance.PlayerStats;
		_playerControlManager = GameSettings.Instance.PlayerControlManager;
		buyMenu.SetActive(false);

		UIBuyingMenuController.tryPurchase += BuyItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public void OpenBuyingMenu()
	{
		_playerControlManager.UnlockCursor();
		buyMenu.SetActive(true);
	}
	
	public void CloseBuyingMenu()
	{
		_playerControlManager.LockCursor();
		buyMenu.SetActive(false);
	}


	public void BuyItem(int itemId)
	{
		int itemValue = _itemManager.GetItem(itemId).gameObject.GetComponent<Item>().Price;

		//Check if the player can buy the item
		if(_playerStats.Money >= itemValue)
		{
			_playerStats.RemoveMoney(itemValue);
			SpawnItem(itemId);

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
