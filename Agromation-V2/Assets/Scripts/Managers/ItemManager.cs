using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemManager")]
public class ItemManager : ScriptableObject
{
	/// <summary>
	/// Script Purpose: Act as a container for all interactable/equiptable items in game,
	/// and provide helper scripts to get information about them
	/// </summary>


	#region Fields
	[SerializeField] private Dictionary<int, GameObject> _items = new Dictionary<int, GameObject>();
	[SerializeField] private AllGameItems itemIndex;

	//Define Singelton
	private static ItemManager _instance;


	#endregion


	#region Properties
	//Properties
	public int ItemCount { get { return _items.Count; } }

	public static ItemManager Instance { get { return _instance; }}

	#endregion


	#region Methods
	private void Awake()
	{ 
		if(_instance == null)
		{
			//Create singleton Instance
			_instance = this;
		}
		//Get the ItemIndex from the GameSettings
		

		//Confirm that all items have been loaded
		//Debug.Log("All items Loaded, final count:" + _items.Count);
	}

	/// <summary>
	/// Populates list if all InGame items
	/// </summary>
	/// <param name="ids">The list of item ids</param>
	/// <param name="items">The list of item prefabs</param>
	public void FillGameItems()
	{
		itemIndex = GameSettings.Instance.ItemIndex;
		//Fill item dictionary
		//Debug.Log("Filling Items");
		for (int i = 0; i < itemIndex.Items.Count; i++)
		{
			//Add an item into the list
			_items.Add(itemIndex.Items[i].GetComponent<Item>().Id, itemIndex.Items[i]);

			//Debug.Log("Item id:" + itemIndex.Items[i].GetComponent<Item>().Id);
			//Debug.Log("Item added to master index:" + itemIndex.items[i].name);
		}
	}

	/// <summary>
	/// Get the object prefab with a specified ID
	/// </summary>
	/// <param name="objectId">The ID of the object to retrieve</param>
	/// <returns>The GameObject prefab with the specified ID </returns>
	public GameObject GetItem(int objectId)
	{
		if (_items.ContainsKey(objectId))
		{
			return _items[objectId];
		}
		else
		{
			return null;
		}
	}

	public bool ValidItem(GameObject itemToLookUp)
	{
		if(_items.ContainsValue(itemToLookUp))
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	/// <summary>
	/// Check if an itemId is valid
	/// </summary>
	/// <param name="itemId">The id to check</param>
	/// <returns>True if the itemId is paired with a valid item</returns>
	public bool ValidItem(int itemId)
	{
		if (_items.ContainsKey(itemId))
		{
			return true;
		}
		else
		{
			//Debug.Log("Contains " + itemId);
			return false;
		}
	}

	/// <summary>
	/// Returns the ID of a gameObject if its part of the master object collection
	/// </summary>
	/// <param name="objectToRetrieveId">The gameObject to find the ID for</param>
	/// <returns>The id(int) of the gameobject passed in</returns>
	public int GetIdOfItem(GameObject itemObjectToRetrieveId)
	{
		if (_items.ContainsValue(itemObjectToRetrieveId))
		{
			for (int i = 0; i < _items.Count; i++)
			{
				if(_items[i] == itemObjectToRetrieveId)
				{
					return i;
				}
			}
		}
		//If it's not found
		return -1;
	}

}
#endregion