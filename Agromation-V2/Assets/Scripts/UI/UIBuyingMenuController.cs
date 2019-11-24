using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyingMenuController : MonoBehaviour
{
	[SerializeField] private List<int> buyableItems;
	[SerializeField] private Scrollbar selectionScrollBar;
	[SerializeField] private GameObject selectionListHolder;
	[SerializeField] private GameObject selectionButtonPrefab;
	[SerializeField] List<Button> selectionButtons = new List<Button>();

	[SerializeField] private ItemManager _itemManager;


	[SerializeField] private Image displayImage;
	[SerializeField] private Text selectedItemName;
	[SerializeField] private Text selectedItemValue;
	//[SerializeField] private

	// Start is called before the first frame update
	void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		for (int i = 0; i < buyableItems.Count; i++)
		{
			//Create a new button
			Button newButton = Instantiate(selectionButtonPrefab, selectionListHolder.transform).GetComponent<Button>();
			selectionButtons.Add(newButton);
			Button newButtonScript = selectionButtons[i];
		}
		for (int i = 0; i < selectionButtons.Count; i++)
		{
			//Set the button name
			selectionButtons[i].GetComponentInChildren<Text>().text = _itemManager.GetItem(buyableItems[i]).GetComponent<Item>().ItemName;

			selectionButtons[i].GetComponent<Button>().onClick.AddListener(() => SelectItem(buyableItems[i]));
			//Registers the OnClick event of the button to the SelectItem Method
		}
	}

	public void Print()
	{
		Debug.Log("asdasda");
	}

	/// <summary>
	/// Sets the display properties of the selectedItem
	/// </summary>
	/// <param name="id">The id of the item Selected</param>
	public void SelectItem(int id)
	{
		Debug.Log("Selected Button!" + id);
		//Item selectedItem = _itemManager.GetItem(id).GetComponent<Item>();

		//displayImage.sprite = selectedItem.Icon;
		//selectedItemName.text = selectedItem.ItemName;
		//selectedItemValue.text = "Price :" + selectedItem.Value.ToString(); 
	}


}
