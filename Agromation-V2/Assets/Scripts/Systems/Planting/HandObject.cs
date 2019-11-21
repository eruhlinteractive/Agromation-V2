using System.Collections;
using System.Collections.Generic;
using UnityEngine;



enum ObjectTypeInHand
{
	Item,
	Tool
}
public class HandObject : MonoBehaviour
{

	GameObject objInHand = null;

	ToolManager _toolManager = null;
	ItemManager _itemManager = null;
	[SerializeField] Transform handPosition = null;

	private ObjectTypeInHand typeInHand = ObjectTypeInHand.Item;


	private GameObject currentItemInHand = null;
	private GameObject currentToolInHand = null;

	//Delegates
	public delegate void IsHoldingTool(bool isHoldingTool);
	public static IsHoldingTool currentlyHoldingTool;


	public delegate void PlacedItem(int itemId);
	public static PlacedItem itemPlaced;


	//Singelton
	private static HandObject instance;
	public static HandObject Instance { get { return instance; } }

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
	// Start is called before the first frame update
	void Start()
    {
		_toolManager = GameSettings.Instance.ToolManager;
		_itemManager = GameSettings.Instance.ItemManager;
	}

    // Update is called once per frame
    void Update()
    {
		//Toggle between holding item and holding tool
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (typeInHand == ObjectTypeInHand.Item)
			{
				typeInHand = ObjectTypeInHand.Tool;
				currentlyHoldingTool(true);
			}
			else
			{
				typeInHand = ObjectTypeInHand.Item;
				currentlyHoldingTool(false);
			}

			SetObjectsAsActive();
		}

		//If a click action is made
		if (Input.GetButtonDown("Fire1"))
		{
			if(typeInHand == ObjectTypeInHand.Item)
			{
				//Check if its a seedpack

				//If YES, check if player is looking at a plot of land

				//IF YES, plant the seed and remove from inventory
			}

		}
    }


	/// <summary>
	/// Sets the current item/tool to active based on what is currently being "held"
	/// </summary>
	public void SetObjectsAsActive()
	{
		if (typeInHand == ObjectTypeInHand.Item)
		{
			if (currentToolInHand != null)
				currentToolInHand.SetActive(false);

			if (currentItemInHand != null)
				currentItemInHand.SetActive(true);
		}
		else if(typeInHand == ObjectTypeInHand.Tool)
		{
			if(currentItemInHand != null)
				currentItemInHand.SetActive(false);

			if(currentToolInHand != null)
				currentToolInHand.SetActive(true);
		}
	}

	/// <summary>
	/// Sets the current item that should be displayed as being held
	/// </summary>
	/// <param name="itemId">The id of the item to hold</param>
	public void SetCurrentItem(int itemId)
	{
		Object.Destroy(currentItemInHand);
		if (_itemManager.ValidItem(itemId))
		{
			currentItemInHand = Instantiate(_itemManager.GetItem(itemId), handPosition);
		}
		SetObjectsAsActive();
	}

	/// <summary>
	/// Sets the current tool that should be displayed as being held
	/// </summary>
	/// <param name="tool">The tool object to set as the current tool.</param>
	public void SetCurrentTool(GameObject tool)
	{
		Object.Destroy(currentToolInHand);
		currentToolInHand = Instantiate(tool, handPosition);
	}
}
