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

	[SerializeField] private SoundController sc;


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
		sc = GetComponent<SoundController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Cursor.lockState == CursorLockMode.Locked)
		{
			//General interaction
			if (_playerLookRayCast.LookHit.collider != null)
			{
				//if The player is looking at a plot
				if (_playerLookRayCast.LookHit.collider.CompareTag("Plot"))
				{
					if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<Plot>().IsPlanted)
					{
						_playerLookRayCast.LookHit.collider.gameObject.GetComponent<Plot>().ShowProgress();
					}
				}

				//Buying Station
				else if (_playerLookRayCast.LookHit.collider.gameObject.CompareTag("BuyingStation"))
				{
					if (Input.GetButton("Fire1"))
					{
						_playerLookRayCast.LookHit.collider.gameObject.GetComponent<BuyingManager>().OpenBuyingMenu();
					}
				}

				//Crafting station
				else if (_playerLookRayCast.LookHit.collider.gameObject.CompareTag("CraftingStation"))
				{
					if (Input.GetButton("Fire1"))
					{
						_playerLookRayCast.LookHit.collider.gameObject.GetComponent<CraftingManager>().OpenCraftingMenu();
					}
				}

				//If not in tool mode
				if (canUseItem)
				{
					//Programming Stations
					if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneStation>() != null)
					{
						if (Input.GetButtonDown("Fire1"))
						{
							_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneStation>().OpenConsole();
						}
					}

					//Storage Interaction
					else if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<StorageChest>() != null)
					{
						//Try placing item in storage
						if (Input.GetButtonDown("Fire2") && _playerInv.currentSelectedId > 2)
						{
							StorageChest chest = _playerLookRayCast.LookHit.collider.gameObject.GetComponent<StorageChest>();
							if (chest.AddItem(_playerInv.currentSelectedId))
							{
								_playerInv.RemoveFromInventory(_playerInv.currentSelectedId);
							}

						}
						//Remove item from storage
						if (Input.GetButtonDown("Fire1"))
						{
							StorageChest chest = _playerLookRayCast.LookHit.collider.gameObject.GetComponent<StorageChest>();
							//Make sure there is something to remove first 
							if (chest.AmountStored != 0)
							{
								chest.RemoveItem();
							}
						}
						return;
					}

					//Drone Module
					else if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneControl>() != null)
					{
						if (Input.GetButtonDown("Fire2"))
						{
							//If the player is holding a drone assignment module
							if (_playerInv.currentSelectedId != -1) //Make sure the player is holding something
								if (_itemManager.GetItem(_playerInv.currentSelectedId).GetComponent<DroneAssignmentModule>() != null)
								{

									DroneControl cont = _playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneControl>();
									GameObject objToAdd;

									//Determine which object should be added
									if (cont.TypeOfDrone == DroneType.Ground)
									{
										objToAdd = _itemManager.GetItem(_playerInv.currentSelectedId).GetComponent<DroneAssignmentModule>().GroundDroneObject;
									}
									else
									{
										objToAdd = _itemManager.GetItem(_playerInv.currentSelectedId).GetComponent<DroneAssignmentModule>().FlyingDroneObject;
									}

									//Is the module able to be successfully added?
									if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneControl>().AddAssignmentModule(_playerInv.currentSelectedId, objToAdd))
									{
										//If so, remove the module from the players inventory
										_playerInv.RemoveFromInventory(_playerInv.currentSelectedId);
									}
								}
						}
					}

					//End Case (regular Pickup/Drop)
					else
					{
						if (Input.GetButtonDown("Fire1"))
						{
							Pickup();
						}
						else if (Input.GetButtonDown("Fire2"))
						{
							Drop();
						}
					}

				}

			}

			//Throw in air
			else if (Input.GetButtonDown("Fire2"))
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
		if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<Item>() != null)
		{
			int id = _playerLookRayCast.LookHit.collider.gameObject.GetComponent<Item>().Id;
			//Debug.Log("It's an item with id:" + id);
			if (_playerInv.AddToInventory(id))
			{
				//Only destroy if item has been successfully added
				Destroy(_playerLookRayCast.LookHit.collider.gameObject);

				float num = Random.value;
				if (num > 0.5f)
				{
					sc.PlaySound("pop_1");
				}
				else
				{
					sc.PlaySound("pop_2");

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
						_playerInv.RemoveFromInventory(itemId);
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

			//Is the item a placable	
			else if (itemId >= 600 && itemId <= 799)
			{
				if (_playerLookRayCast.LookHit.collider != null)
				{
					
					//Is the player holding a drone item?
					if (itemId == 603 || itemId == 604)
					{
						//Are they looking at a drone pad
						if(_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DronePad>() != null)
						{
							//Debug.Log(_playerLookRayCast.LookHit.collider.gameObject.name);
							//Make sure there isnt already a drone linked to the pad
							if(_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DronePad>().LinkedDrone == null)
							{
								PlaceDrone(_itemManager.GetItem(itemId).GetComponent<IPlacable>().PlacedObject,
								_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DronePad>());
								_playerInv.RemoveFromInventory(itemId);
							}
						}
						//Player isnt looking at a drone pad and can't place drone
						else
						{
							ThrowObject();
						}
					}

					//If they are NOT holding a drone item
					//Get placable item and the point to place at
					else if (_playerLookRayCast.LookHit.collider.CompareTag("Ground") && (Vector3.Distance(transform.position, _playerLookRayCast.LookHit.point) < 5f))
					{
						PlacePlaceable(_itemManager.GetItem(itemId).GetComponent<IPlacable>().PlacedObject, _playerLookRayCast.LookHit.point, itemId);

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
		_playerInv.RemoveFromInventory(_playerInv.currentSelectedId);
		sc.PlaySound("drop_item");
	}

	/// <summary>
	/// Creates a plant if the player is holding a seed pack
	/// </summary>
	/// <param name="plot">The plot to plant it in</param>
	/// <param name="plantId">Which plant to plant, based on seed pack field</param>
	private void Plant(GameObject plot, int plantId)
	{
		//Get the plot object
		
		GameObject plant = _itemManager.GetItem(plantId);
		GameObject newPlant = Instantiate(plant,
			plot.transform.position + Vector3.up * 0.25f + (plant.transform.GetChild(0).localScale.y / 2 * Vector3.up),
			Quaternion.identity,
			plot.transform);
		//Send the plot its new plant reference
		plot.GetComponent<Plot>().Plant(newPlant);
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

		//Check if the land plot is open
		if (_plotManager.OpenSpot(targetPosition))
		{
			GameObject newPlacable = Instantiate(placeableObject, targetPosition + placeableObject.transform.localScale.y/2 * Vector3.up,Quaternion.identity);
			newPlacable.transform.rotation = NearestCardinalRotation();	//Set the rotation of the new object
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
		Vector3 alignedForward = NearestWorldAxis(transform.forward);
		Vector3 alignedUp = NearestWorldAxis(transform.up);

		//-alignedForward - Face player
		Quaternion rotation = Quaternion.LookRotation(-alignedForward, alignedUp);
		return rotation;
		
	}

	/// <summary>
	/// Finds the neares world axis to the v parameter
	/// </summary>
	/// <param name="v">The vector to find the nearest world axis to</param>
	/// <returns> A vector3 of the nearest world vector</returns>
	private Vector3 NearestWorldAxis(Vector3 v)
	{
		if (Mathf.Abs(v.x) < Mathf.Abs(v.y))
		{
			v.x = 0;
			if (Mathf.Abs(v.y) < Mathf.Abs(v.z))
				v.y = 0;
			else
				v.z = 0;
		}
		else
		{
			v.y = 0;
			if (Mathf.Abs(v.x) < Mathf.Abs(v.z))
				v.x = 0;
			else
				v.z = 0;
		}
		return v;
	}

}
