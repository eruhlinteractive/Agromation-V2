using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSwap : MonoBehaviour
{
	#region Fields
	private ToolManager _toolManager;
	private int currentTool = 0;
	private List<GameObject> unlockedToolList;
	private GameObject currentTool_Obj = null;
	private Transform handPosition = null;
	private bool toolMode = false;

	//Delegates
	public delegate void NewTool(Tool newTool);
	public static NewTool displayNewTool;


	

	#endregion
	public bool ToolMode { get { return toolMode; } }

	#region Methods
	// Start is called before the first frame update
	void Start()
    {
		_toolManager = GameSettings.Instance.ToolManager;
		unlockedToolList = _toolManager.UnlockedTools;
		//Bind Delegates
		ToolManager.refreshToolList += RefreshList;
		HandObject.currentlyHoldingTool += ToggleToolMode;


		currentTool_Obj = unlockedToolList[0];
	}

    // Update is called once per frame
    void Update()
    {
		
			ChangeTool();
	}

	/// <summary>
	/// Wraps the "currentTool" int to keep it within the bounds of the unlockedToolList
	/// </summary>
	private void CurrentToolWrap()
	{
		if(currentTool < 0)
		{
			currentTool = unlockedToolList.Count-1;
		}
		if(currentTool >= unlockedToolList.Count)
		{
			currentTool = 0;
		}
	}

	/// <summary>
	/// Changes the current Tool
	/// </summary>
	private void ChangeTool()
	{
		if (Cursor.lockState == CursorLockMode.Locked)
		{
			if (toolMode)
			{
				if (Input.GetKeyDown(KeyCode.Q))
				{
					currentTool--;
					SetCurrentTool();
				}
				if (Input.GetKeyDown(KeyCode.E))
				{
					currentTool++;
					SetCurrentTool();
				}
			}
		}
	}


	/// <summary>
	/// Returns the currently equipped Tool
	/// </summary>
	/// <returns>The currently equipped tool</returns>
	public GameObject GetEquippedTool()
	{
		return unlockedToolList[currentTool];
	}

	/// <summary>
	/// Refreshes the local tool list
	/// </summary>
	private void RefreshList()
	{
		unlockedToolList = _toolManager.UnlockedTools;
	}


	/// <summary>
	/// Updated data to reflect a different active tool
	/// </summary>
	private void SetCurrentTool()
	{
		CurrentToolWrap();
		//Call delegate to update the tool icon
		displayNewTool(unlockedToolList[currentTool].GetComponent<Tool>());
		currentTool_Obj = unlockedToolList[currentTool];
		HandObject.Instance.SetCurrentTool(currentTool_Obj);
	}

	/// <summary>
	/// Toggles tool mode on and off
	/// </summary>
	private void ToggleToolMode(bool inToolMode)
	{
		toolMode = inToolMode;
	}

	#endregion
}
