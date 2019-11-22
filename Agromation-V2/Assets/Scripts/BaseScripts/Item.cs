using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item : MonoBehaviour
{
	#region Fields
	[SerializeField]private int itemId = -1;
	[SerializeField]private int itemValue = -1;
	[SerializeField]private Sprite itemIcon = null;
	[SerializeField]private string itemName;



	#endregion

	#region Properties
	public int Id { get { return itemId; } }
	public int Value { get {return itemValue; } }
	public Sprite Icon { get { return itemIcon; } }
	public string ItemName { get => itemName; }


	#endregion

	private void Awake()
	{
		gameObject.tag = "Item";
	}
}
