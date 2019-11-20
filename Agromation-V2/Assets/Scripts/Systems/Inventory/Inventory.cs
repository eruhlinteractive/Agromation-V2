using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

	/// <summary>
	/// Purpose: Interfaces with PlayerInventory to add/remove items
	/// </summary>
	[SerializeField]private ItemManager _itemManager;
	[SerializeField] private PlayerInventory _playerInv;
	[SerializeField] private GameObject playerHead;
	[SerializeField] private int lookDistance;
	RaycastHit lookHit;

	// Start is called before the first frame update
	void Start()
	{
		//Get the singelton instance of the ItemManager

		_itemManager = GameSettings.Instance.ItemManager;
		_playerInv = GameSettings.Instance.PlayerInventory;
		
		
	}

    // Update is called once per frame
    void Update()
    {
		Pickup();

		Debug.DrawRay(playerHead.transform.position, playerHead.transform.forward * lookDistance, Color.red);
	}

	/// <summary>
	/// Attempts to add item to the players inventory
	/// </summary>
	/// <param name="itemId">The id of the item that should be added</param>
	/// <returns>True if the item was successfully added the inventory</returns>
	public bool TryAddToInventory(int itemId)
	{
		return _playerInv.AddToInventory(itemId);
	}

	void Pickup()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			//Make sure the player is looking at something
			if (Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, out lookHit, lookDistance))
			{
				Debug.Log("Hit " + lookHit.collider.name);
				// If the hit collider has an item script attached
				if (lookHit.collider.gameObject.CompareTag("Item"))
				{
					int id = lookHit.collider.gameObject.GetComponent<Item>().Id;

					//Check to see if it is already in the inventory
					if (_playerInv.IsItemInInventory(id))
					{
						_playerInv.AmountInInventory(id);
					}
					//Try adding item
					else if(_playerInv.AddToInventory(id))
					{
						Debug.Log("It's an item!");
						//Only destroy if item has been successfully added
						Destroy(lookHit.collider.gameObject);
					}
				}

			}

		}
	}
}
