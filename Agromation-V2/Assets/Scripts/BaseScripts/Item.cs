using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
	#region Fields
	[SerializeField]private int itemId = -1;
	[SerializeField]private int itemValue = -1;
	[SerializeField]private Sprite itemIcon = null;


	#endregion

	#region Properties
	public int Id { get { return itemId; } }
	public int Value { get {return itemValue; } }
	public Sprite Icon { get { return itemIcon; } }


	#endregion

	private void Awake()
	{
		this.tag = "Item";
	}
}
