using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

	/// <summary>
	/// Purpose: Interfaces with PlayerInventory to add/remove items
	/// </summary>
	[SerializeField]private ItemManager _itemManager = null;
	[SerializeField] private PlayerInventory _playerInv = null;
	[SerializeField] private GameObject playerHead = null;
	[SerializeField] private int lookDistance = 0;
	PlayerLookRayCast _playerLookRayCast;

	// Start is called before the first frame update
	void Start()
	{
		//Get the singelton instance of the ItemManager
		_itemManager = GameSettings.Instance.ItemManager;
		_playerInv = GameSettings.Instance.PlayerInventory;
		_playerLookRayCast = PlayerLookRayCast.Instance;
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

	}

	/// <summary>
	/// Runs object pickup proccess
	/// </summary>
	private void Pickup()
	{
		if(_playerLookRayCast.LookHit.collider != null)
		{
			//If the hit collider has an item script attached
			if (_playerLookRayCast.LookHit.collider.gameObject.CompareTag("Item"))
			{
				int id = _playerLookRayCast.LookHit.collider.gameObject.GetComponent<Item>().Id;
				//Debug.Log("It's an item with id:" + id);
				if (_playerInv.AddToInventory(id))
				{
					//Only destroy if item has been successfully added
					Destroy(_playerLookRayCast.LookHit.collider.gameObject);
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
