using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
	private Text moneyText;
    // Start is called before the first frame update
    void Start()
    {
		moneyText = gameObject.GetComponent<Text>();
		PlayerStats.updateMoneyUi += UpdateMoneyAmount;
    }

	private void UpdateMoneyAmount(int newAmount)
	{
		moneyText.text = newAmount.ToString("D6");
	}
}
