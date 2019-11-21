using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
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
		if (Input.GetKeyDown(KeyCode.F))
		{
			Pickup();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			Drop();
		}

		Debug.DrawRay(playerHead.transform.position, playerHead.transform.forward * lookDistance, Color.red);
	}

	/// <summary>
	/// Runs object pickup proccess
	/// </summary>
	private void Pickup()
	{
		//Make sure the player is looking at something
		if (Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, out lookHit, lookDistance))
		{
			//Debug.Log("Hit " + lookHit.collider.name);
			// If the hit collider has an item script attached
			if (lookHit.collider.gameObject.CompareTag("Item"))
			{
				int id = lookHit.collider.gameObject.GetComponent<Item>().Id;
				//Debug.Log("It's an item with id:" + id);
				if (_playerInv.AddToInventory(id))
				{
					//Only destroy if item has been successfully added
					Destroy(lookHit.collider.gameObject);
				}
			}

		}
	}

	/// <summary>
	/// "Drops" selected object if it is still in the inventory
	/// </summary>
	private void Drop()
	{
		//Make sure the player is looking at something
		
		if(_playerInv.AmountInInventory(_playerInv.currentSelectedId) > 0)
		{
			GameObject placedObject = Instantiate(_itemManager.GetItem(_playerInv.currentSelectedId),
				playerHead.transform.position + (playerHead.transform.forward * 2),
				Quaternion.identity);

			//"Throw" instantiated object
			placedObject.GetComponent<Rigidbody>().AddForce(playerHead.transform.forward * 2, ForceMode.Impulse);
			_playerInv.RemoveFromInventory(_playerInv.currentSelectedId);

			
		}
	}
}
