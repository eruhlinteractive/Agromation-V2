using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Player Stats")]
public class PlayerStats : ScriptableObject
{
	private int money = 0;
	//private int health = 100;

	//public int Coins { get { return coins; } }

	//Called to update money UI
	public delegate void MoneyChange(int currentCoinAmount);
	public static MoneyChange updateMoneyUi;

	public void Initialize()
	{
		SetInitalValue();
	}

	private void SetInitalValue()
	{
		money = 0;
	}

	/// <summary>
	/// Adds to players "money"
	/// </summary>
	/// <param name="amountToAdd">The amount to add to to the money total</param>
	public void AddMoney(int amountToAdd)
	{
		money += amountToAdd;
		updateMoneyUi(money);
	}

	/// <summary>
	/// Removes from players "money"
	/// </summary>
	/// <param name="amountToRemove">The amount to remove from the money total</param>
	public void RemoveMoney(int amountToRemove)
	{
		money -= amountToRemove;
		updateMoneyUi(money);
	}
}
