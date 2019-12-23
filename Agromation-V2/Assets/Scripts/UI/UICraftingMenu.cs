using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICraftingMenu : MonoBehaviour
{
	[SerializeField] private List<int> craftableItems;
	[SerializeField] private Slider selectionScrollBar;
	[SerializeField] private GameObject selectionListHolder;
	[SerializeField] private GameObject selectionButtonPrefab;
	[SerializeField] List<Button> selectionButtons = new List<Button>();
	[SerializeField] private Vector3 selectionHolderInitialPosition;

	[SerializeField] private ItemManager _itemManager;
	[SerializeField] private int currentlySelectedItem;

	[SerializeField] private Image displayImage;
	[SerializeField] private Text selectedItemName;
	[SerializeField] private Text selectedItemRecipe;

	[SerializeField] CraftingManager currentlyActiveManager = null;
	// Start is called before the first frame update
	void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		UICraftingOptionButton.clicked += SelectItem;
		
	}

	/// <summary>
	/// Check if the menu needs to be repopulated
	/// </summary>
	/// <param name="currentCraftingStation">The crafting station the player is currently at</param>
	public void MenuOpened(CraftingManager currentCraftingStation)
	{
		if(currentCraftingStation != currentlyActiveManager)
		{
			if(_itemManager == null)
			{
				_itemManager = GameSettings.Instance.ItemManager;
			}
			currentlyActiveManager = currentCraftingStation;
			ChangeMenu();
		}
		
	}

	/// <summary>
	/// Try to craft the currently selected object
	/// </summary>
	public void TryCraft()
	{
		currentlyActiveManager.TryCrafting(currentlySelectedItem);
	}
	/// <summary>
	/// Run the proccess to change between different crafting menus (i.e Crafting bench & furnace)
	/// </summary>
	private void ChangeMenu()
	{
		//Clear old data from UI
		craftableItems = currentlyActiveManager.CraftableItems;
		selectionButtons.Clear();


		//Loop through an destroy each child
		for (int i = 0; i < selectionListHolder.transform.childCount; i++)
		{
			Destroy(selectionListHolder.transform.GetChild(i).gameObject);
		}

		//Create a button for each craftable item and set the item id they trigger
		for (int i = 0; i < craftableItems.Count; i++)
		{
			//Create a new button
			Button newButton = Instantiate(selectionButtonPrefab, selectionListHolder.transform).GetComponent<Button>();
			selectionButtons.Add(newButton);

			//Set the button id and image
			newButton.GetComponentInChildren<Image>().sprite = _itemManager.GetItemIcon(craftableItems[i]);
			newButton.GetComponent<UICraftingOptionButton>().buttonId = craftableItems[i];

		}
		//Set icon and name
		displayImage.sprite = null ;
		selectedItemName.text = "None Selected";

		//Set the text field
		selectedItemRecipe.text = "";

		SetIcons();
	}

	/// <summary>
	/// Sets the Icons of each selection button in the menu
	/// </summary>
	private void SetIcons()
	{
		for (int i = 0; i < selectionButtons.Count; i++)
		{
			GameObject button = selectionButtons[i].gameObject;
		}
	}

	/// <summary>
	/// Update the display to show the currently selected button
	/// </summary>
	/// <param name="id">The itemId of the selected object</param>
	private void SelectItem(int id)
	{
		currentlySelectedItem = id;

		CraftableObject selectedItem = _itemManager.GetItem(id).GetComponent<CraftableObject>();

		//Set icon and name
		displayImage.sprite = selectedItem.Icon;
		selectedItemName.text = selectedItem.ItemName;

		string itemRecipe = "";

		//Loop through each ingredient, extracting name and amount needed
		for (int i = 0; i < selectedItem.Ingredients.Count; i++)
		{
			itemRecipe += _itemManager.GetItem(selectedItem.Ingredients[i]).GetComponent<Item>().ItemName 
				+ " x" 
				+ selectedItem.GetIngredientAmount(selectedItem.Ingredients[i])
				+ "\n";
		}
		//Set the text field
		selectedItemRecipe.text = itemRecipe;
	}

	public void CloseMenu()
	{
		currentlyActiveManager.CloseCraftingMenu();
	}
}
