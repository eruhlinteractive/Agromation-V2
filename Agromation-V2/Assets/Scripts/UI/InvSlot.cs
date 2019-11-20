using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour
{
	#region Fields
	[SerializeField] private int id = -1;
	[SerializeField] private int amount = 0;
	[SerializeField] private Sprite icon;


	//Ui elements
	[SerializeField] private Image imageObj;
	[SerializeField] private Text amountText;

	public int Id { get => id; }

	#endregion


	#region Methods
	// Start is called before the first frame update
	void Start()
    {
		//Fill UI elements
		imageObj = this.GetComponent<Image>();
		amountText = this.GetComponentInChildren<Text>();

		PurgeData();
    }

	/// <summary>
	/// Sets initial values when slot is first bound to an item Id
	/// </summary>
	/// <param name="id">The id of the item bound to the slot</param>
	/// <param name="amount">How many are in the inventory currently</param>
	/// <param name="icon">The icon of the item in the inventory</param>
	public void SetData(int id, int amount, Sprite icon)
	{
		this.id = id;
		this.amount = amount;
		this.icon = icon;
		this.imageObj.sprite = icon;
		amountText.text = amount.ToString();
	}

	/// <summary>
	/// Sets the current amount in the inventory and updates graphical text
	/// </summary>
	/// <param name="newAmount">The new amount in the inventory</param>
	public void SetAmount(int newAmount)
	{
		this.amount = newAmount;
		amountText.text = amount.ToString();

		//Remove the slot
		if(newAmount <= 0)
		{
			PurgeData();
		}
	}
	
	/// <summary>
	/// Clears data in the slot, alowing it to be used again
	/// </summary>
	public void PurgeData()
	{
		this.id = -1;
		this.amount = 0;
		this.icon = null;
		this.amountText.text = amount.ToString();
		this.imageObj.sprite = null;
	}

	#endregion
}
