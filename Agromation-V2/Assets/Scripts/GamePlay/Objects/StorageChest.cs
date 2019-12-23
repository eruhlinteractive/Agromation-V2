using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageChest : MonoBehaviour
{

	private int idOfStoredItem = -1;
	[SerializeField] private int amountStored;
	[SerializeField] Text amountStoredText;
	[SerializeField] Image itemStoredIcon;
	[SerializeField] GameObject storedDisplay;
	[SerializeField] Transform objectSpawnPos;
	private ItemManager _itemManager;
	private PlayerInventory _playerInventory;
	Vector3 mainCam;
	[SerializeField] float spawnForce;
	[SerializeField] Animator chestAnim;


	//Properties
	public int IdOfStoredItem { get => idOfStoredItem; }
	public int AmountStored { get => amountStored;}

	// Start is called before the first frame update
	void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		_playerInventory = GameSettings.Instance.PlayerInventory;

	}
	private void Update()
	{
		mainCam = Camera.main.transform.position;
		if (Vector3.Distance(transform.position, mainCam) <6)
		{
			chestAnim.SetBool("Opened", true);
		}
		
		if(Vector3.Distance(transform.position,mainCam) < 10 )
		{
			if(amountStored != 0)
			{
				//Billboard to player
				storedDisplay.SetActive(true);
				storedDisplay.transform.LookAt(mainCam);
			}
			
		}
		else
		{
			storedDisplay.SetActive(false);
			chestAnim.SetBool("Opened", false);
		}


		

	}
	/// <summary>
	/// Try adding an item to the chest
	/// </summary>
	/// <param name="itemId">The id of the item to try adding</param>
	/// <returns>True if the item was successfully added</returns>
	public bool AddItem(int itemId)
	{
		//Already items stored
		if (idOfStoredItem == itemId)
		{
			amountStored++;
			amountStoredText.text = "x" + amountStored.ToString();
			return true;
		}

		//Nothing is stored
		else if (idOfStoredItem == -1)
		{
			//storedDisplay.SetActive(true);
			idOfStoredItem = itemId;
			itemStoredIcon.sprite = _itemManager.GetItemIcon(itemId);
			amountStored = 1;
			amountStoredText.text = "x" + amountStored.ToString();
			return true;
		}
		return false;
	}

	/// <summary>
	/// Remove one of the stored items
	/// </summary>
	public void RemoveItem()
	{
		//Try adding it directly to inventory
		if (!_playerInventory.AddToInventory(idOfStoredItem))
		{
			//Spawn new item
			if (_itemManager.GetItem(idOfStoredItem) != null)
			{
				Rigidbody newItem = Instantiate(_itemManager.GetItem(idOfStoredItem), objectSpawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
				newItem.AddForce(objectSpawnPos.transform.forward * spawnForce, ForceMode.Impulse);
			}
		}
		amountStored--;
		amountStoredText.text = "x" + amountStored.ToString();
		if (amountStored < 1)
		{
			Reset();
		}
		//Its already in the players inventory

	}

	/// <summary>
	/// Reset the data of the chest
	/// </summary>
	private void Reset()
	{
		
		idOfStoredItem = -1;
		itemStoredIcon.sprite = null;
		amountStored = 0;
		amountStoredText.text = "x" + amountStored.ToString();
		storedDisplay.SetActive(false);
	}
}
