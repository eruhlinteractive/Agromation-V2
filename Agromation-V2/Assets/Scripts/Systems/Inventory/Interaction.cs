using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

	/// <summary>
	/// Purpose: Interfaces with PlayerInventory to add/remove items
	/// </summary>
	[SerializeField] private ItemManager _itemManager = null;
	[SerializeField] private PlayerInventory _playerInv = null;
	private PlotManager _plotManager = null;
	private Grid _grid;
	[SerializeField] private GameObject playerHead = null;
	[SerializeField] private int lookDistance = 0;
	PlayerLookRayCast _playerLookRayCast;

	private bool canUseItem = true;

	// Start is called before the first frame update
	void Start()
	{
		//Get the singelton instance of the ItemManager
		_itemManager = GameSettings.Instance.ItemManager;
		_playerInv = GameSettings.Instance.PlayerInventory;
		_playerLookRayCast = PlayerLookRayCast.Instance;
		HandObject.currentlyHoldingTool += IsHoldingTool;
		_plotManager = PlotManager.Instance;
		_grid = Grid.Instance;
	}

	// Update is called once per frame
	void Update()
	{
		if (Cursor.lockState == CursorLockMode.Locked)
			//General interaction
			if (_playerLookRayCast.LookHit.collider != null)
			{
				//if The player is looking at a plot
				if (_playerLookRayCast.LookHit.collider.CompareTag("Plot"))
				{
					//_playerLookRayCast.LookHit.collider.gameObject.GetComponent<Plot>().GrowthDisplay.gameObject.SetActive(true);
				}

				//Buying Station
				if (_playerLookRayCast.LookHit.collider.gameObject.CompareTag("BuyingStation"))
				{
					if (Input.GetButton("Fire1"))
					{
						_playerLookRayCast.LookHit.collider.gameObject.GetComponent<BuyingManager>().OpenBuyingMenu();
					}
				}

				//If not in tool mode
				if(canUseItem)
				{
					//Programming Stations
					if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneStation>() != null)
				{
					if (Input.GetButton("Fire1"))
					{
						_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneStation>().OpenConsole();
					}
				}
				}

			}

		//First make sure that the player is holding an item (not a tool)
		if (canUseItem)
		{
			if (_playerLookRayCast.LookHit.collider != null)
				if (Input.GetButtonDown("Fire1"))
				{
					Pickup();
				}
			if (Input.GetButtonDown("Fire2"))
			{
				Drop();
			}
		}
	}


	/// <summary>
	/// Runs object pickup proccess
	/// </summary>
	private void Pickup()
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
			//Placable
			else if (itemId >= 600 && itemId <= 799)
			{
				if (_playerLookRayCast.LookHit.collider != null)
				{
					//Get placable item and the point to place at
					if (_playerLookRayCast.LookHit.collider.CompareTag("Ground") && (Vector3.Distance(transform.position, _playerLookRayCast.LookHit.point) < 5f))
					{

						PlacePlaceable(_itemManager.GetItem(itemId).GetComponent<Placable>().PlacableObject, _playerLookRayCast.LookHit.point,itemId);
						
					}
					//Is the player holding a drone item?
					else if (itemId == 603 || itemId == 604)
					{
						if(_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DronePad>() != null)
						{
							PlaceDrone(_itemManager.GetItem(itemId).GetComponent<Placable>().PlacableObject,
								_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DronePad>());
							_playerInv.RemoveFromInventory(itemId);
						}
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
		GameObject plant = _itemManager.GetItem(plantId);
		GameObject newPlant = Instantiate(plant,
			plot.transform.position + Vector3.up * 0.25f + (plant.transform.GetChild(0).localScale.y / 2 * Vector3.up),
			Quaternion.identity,
			plot.transform);
	}


	/// <summary>
	/// Checks HandObject.cs to see if the player is holding a tool currently
	/// If NOT, then the player uses the item on input
	/// </summary>
	/// <param name="holdingTool">Is the player currently holding a tool (based on HandObject Script</param>
	private void IsHoldingTool(bool holdingTool)
	{
		if (holdingTool)
		{
			canUseItem = false;
		}
		else
		{
			canUseItem = true;
		}

	}



	/// <summary>
	/// Places a placable item
	/// </summary>
	/// <param name="placeableObject">The PLACABLE OBJECT to place at the calculated position</param>
	/// <param name="position">The (RAW) position to place the object</param>
	private void PlacePlaceable(GameObject placeableObject, Vector3 position,int itemId)
	{
		Vector3 targetPosition = _grid.GetNearestPointOnGrid(position);
		if (_plotManager.OpenSpot(targetPosition))
		{
			GameObject newPlacable = Instantiate(placeableObject, targetPosition + placeableObject.transform.localScale.y/2 * Vector3.up, NearestCardinalRotation());
			_plotManager.AddPlot(targetPosition, newPlacable);
			_playerInv.RemoveFromInventory(itemId);
		}
	}
	/// <summary>
	/// Places a drone and links it to the pad it was placed on
	/// </summary>
	/// <param name="placeableObject">The drone to place</param>
	/// <param name="pad">The drone pad to place it on</param>
	private void PlaceDrone(GameObject placeableObject, DronePad pad)
	{
		Vector3 targetPosition = pad.transform.position + (placeableObject.transform.localScale.y /2 * Vector3.up);
		GameObject newDrone = Instantiate(placeableObject, targetPosition, NearestCardinalRotation());
		pad.LinkDrone(newDrone.GetComponent<DroneControl>());
	}

	/// <summary>
	/// Calculates the Cardinal Rotation (multiple of 90) to the players position
	/// </summary>
	/// <returns>The </returns>
	private Quaternion NearestCardinalRotation()
	{
		Vector3 aimingDir = transform.forward;
		float angle = -Mathf.Atan2(aimingDir.z, aimingDir.x) * Mathf.Rad2Deg + 90.0f;
		angle = Mathf.Round(angle / 90.0f) * 90.0f;
		return Quaternion.AngleAxis(-angle, Vector3.up);
		
	}
}
