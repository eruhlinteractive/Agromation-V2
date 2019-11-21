using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSlot : MonoBehaviour
{
	private Image icon;

    // Start is called before the first frame update
    void Start()
    {
		icon = GetComponent<Image>();
		ToolSwap.displayNewTool += ChangeIcon;
    }

    void ChangeIcon(Tool newTool)
	{
		icon.sprite = newTool.Icon;
	}
}
