using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingManager : MonoBehaviour
{
	[SerializeField] private PlayerStats _playerStats;
	[SerializeField] private ItemManager _itemManager;
    // Start is called before the first frame update
    void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		_playerStats = GameSettings.Instance.PlayerStats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public bool BuyItem(int itemId)
	{
		int itemValue = _itemManager.GetItem(itemId).gameObject.GetComponent<Item>().Value;

		//Check if the player can buy the item
		if(_playerStats.Money > itemValue)
		{
			_playerStats.RemoveMoney(itemValue);
			Debug.Log("Can Buy Item");
			return true;
		}
		else
		{
			return false;
		}
	}
}
