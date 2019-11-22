﻿using System.Collections;
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
		if (Input.GetButtonDown("Fire1"))
		{
			Pickup();
		}
		if (Input.GetButtonDown("Fire2"))
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
		int itemId = _playerInv.currentSelectedId;
		//Make sure the player is looking at something

		if (_playerInv.AmountInInventory(itemId) > 0)
		{
			//Plant the item
			//Is it a seed?
			if (itemId > 199 && itemId <= 399)
			{
				//If the player is looking at something
				if (_playerLookRayCast.LookHit.collider != null)
				{
					if (_playerLookRayCast.LookHit.collider.CompareTag("Plot") && !(_playerLookRayCast.LookHit.collider.gameObject.GetComponent<Plot>().IsPlanted))
					{
						Plant(_playerLookRayCast.LookHit.collider.gameObject, _itemManager.GetItem(itemId).GetComponent<SeedPack>().PlantId);
					}
					else
					{
						ThrowObject();
					}
				}
				else
				{
					ThrowObject();
				}
			}
			//Otherwise place the item
			else
			{
				ThrowObject();
			}

			_playerInv.RemoveFromInventory(itemId);
		}
	}

	/// <summary>
	/// Drops the object, adding a slight forward force
	/// </summary>
	private void ThrowObject()
	{
		GameObject placedObject = Instantiate(_itemManager.GetItem(_playerInv.currentSelectedId),
				playerHead.transform.position + (playerHead.transform.forward * 2),
				Quaternion.identity);
		//"Throw" instantiated object
		placedObject.GetComponent<Rigidbody>().AddForce(playerHead.transform.forward * 2, ForceMode.Impulse);

	}

	/// <summary>
	/// Creates a plant if the player is holding a seed pack
	/// </summary>
	/// <param name="plot">The plot to plant it in</param>
	/// <param name="plantId">Which plant to plant, based on seed pack field</param>
	private void Plant(GameObject plot, int plantId)
	{
		//Get the plot object
		plot.GetComponent<Plot>().Plant();
		GameObject newPlant = Instantiate(_itemManager.GetItem(plantId),plot.transform);
		//if (newPlant.GetComponent<Rigidbody>() != ni)
		//newPlant.GetComponent<Plant>().StartGrowing();
		
		//Debug.Log("Plant Seed");
	}
}
