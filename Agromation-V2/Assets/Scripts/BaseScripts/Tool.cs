using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
	[SerializeField] private Sprite icon;
	[SerializeField] private string toolName;

	public string ToolName { get => toolName;}
	public Sprite Icon { get => icon;}
	
}
