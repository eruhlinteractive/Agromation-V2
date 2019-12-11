using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyingOptionButtonUI : MonoBehaviour
{
	public delegate void ButtonClick(int id);
	public static ButtonClick clicked;
	public int buttonId;

	private void OnEnable()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}


	public void OnClick()
	{
		//Debug.Log("Button Fired");
		clicked(buttonId);
	}
}
