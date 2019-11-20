using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	/// <summary>
	/// Purpose: To Hold all scriptable Objects In the game and allow universal access to them
	/// </summary>

	//Singleton
	private static GameSettings instance;
	public static GameSettings Instance { get { return instance; } }


	[SerializeField] private AllGameItems itemIndex;
	public AllGameItems ItemIndex { get { return itemIndex; } }

	[SerializeField] private PlayerInventory playerInventory;
	public PlayerInventory PlayerInventory { get { return playerInventory; } }

	[SerializeField] private ItemManager _itemManager;
	public ItemManager ItemManager { get { return _itemManager; } }


	private void Awake()
	{
		instance = this;

		//Initialize the scriptable objects
		_itemManager.FillGameItems();
		playerInventory.Initalize();
	}


}
