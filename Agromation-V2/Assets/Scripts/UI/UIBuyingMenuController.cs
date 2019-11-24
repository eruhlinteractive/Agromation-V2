using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyingMenuController : MonoBehaviour
{
	#region Fields
	[SerializeField] private List<int> buyableItems;
	[SerializeField] private Slider selectionScrollBar;
	[SerializeField] private GameObject selectionListHolder;
	[SerializeField] private GameObject selectionButtonPrefab;
	[SerializeField] List<Button> selectionButtons = new List<Button>();

	[SerializeField] private Vector3 selectionHolderInitialPosition;
	[SerializeField] private Text purchaseFeedbackText;

	[SerializeField] private ItemManager _itemManager;
	[SerializeField] private int currentlySelectedItem;

	[SerializeField] private Image displayImage;
	[SerializeField] private Text selectedItemName;
	[SerializeField] private Text selectedItemValue;
	//[SerializeField] private

	#endregion


	//Delegate
	public delegate void PurchaseButtonClicked(int currentId);
	public static PurchaseButtonClicked tryPurchase;


	// Start is called before the first frame update
	void Start()
    {

		//Link delegates
		BuyingManager.wasItemBought += PurchaseFeedback;

		BuyingOptionButtonUI.clicked += SelectItem;
		_itemManager = GameSettings.Instance.ItemManager;

		selectionHolderInitialPosition = selectionListHolder.transform.localPosition;


		//Create a button for each buyable item and set the item id they trigger
		for (int i = 0; i < buyableItems.Count; i++)
		{
			//Create a new button
			Button newButton = Instantiate(selectionButtonPrefab, selectionListHolder.transform).GetComponent<Button>();
			selectionButtons.Add(newButton);
			Button newButtonScript = selectionButtons[i];

			//Set the button name
			selectionButtons[i].GetComponentInChildren<Text>().text = _itemManager.GetItem(buyableItems[i]).GetComponent<Item>().ItemName;
			selectionButtons[i].GetComponent<BuyingOptionButtonUI>().buttonId = buyableItems[i];
		}

		selectionScrollBar.maxValue = buyableItems.Count;
	}

	private void Update()
	{
		//Set the position of the menu
		selectionListHolder.transform.localPosition = new Vector3(0, selectionHolderInitialPosition.y +
			 selectionListHolder.GetComponent<RectTransform>().sizeDelta.y / (buyableItems.Count * 2) * selectionScrollBar.value);
	}

	/// <summary>
	/// Sets the display properties of the selectedItem
	/// </summary>
	/// <param name="id">The id of the item Selected</param>
	public void SelectItem(int id)
	{
		Item selectedItem = _itemManager.GetItem(id).GetComponent<Item>();


		//Set Icon Display
		displayImage.sprite = selectedItem.Icon;
		selectedItemName.text = selectedItem.ItemName;
		selectedItemValue.text = "Price :" + selectedItem.Price.ToString();
		currentlySelectedItem = id;
	}

	/// <summary>
	/// Try to purchase the selected item
	/// </summary>
	public void TryPurchase()
	{
		tryPurchase(currentlySelectedItem);
	}

	/// <summary>
	/// Give visual feedback on whether the item was bought successfully
	/// </summary>
	/// <param name="boughtSuccessfully"></param>
	private void PurchaseFeedback(bool boughtSuccessfully)
	{
		//Tell player whether it was successfully bought
		if (boughtSuccessfully)
		{
			purchaseFeedbackText.text = "Successfully bought " + _itemManager.GetItem(currentlySelectedItem).GetComponent<Item>().ItemName +"!";
			purchaseFeedbackText.color = Color.white;
			StartCoroutine(FadeOut());

		}
		else
		{
			//Purchase error
			purchaseFeedbackText.text = "Not enough money!";
			purchaseFeedbackText.color = Color.red;
			StartCoroutine(FadeOut());
		}
		StartCoroutine(FadeOut());
	}

	/// <summary>
	/// Fades out color over time
	/// </summary>
	/// <returns></returns>
	IEnumerator FadeOut()
	{ 
		yield return new WaitForSeconds(1.5f);
		purchaseFeedbackText.CrossFadeColor(Color.clear, 1.5f, true, true);
		
	}

}
