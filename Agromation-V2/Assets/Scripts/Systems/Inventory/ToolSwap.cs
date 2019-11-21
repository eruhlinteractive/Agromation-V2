using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSwap : MonoBehaviour
{
	#region Fields
	private ToolManager _toolManager;
	private int currentTool = 0;
	private List<GameObject> unlockedToolList;

	//Delegates
	public delegate void NewTool(Tool newTool);
	public static NewTool displayNewTool;


	public GameObject currentTool_Obj = null;

	public Transform handPosition;

	private bool toolMode = false;

	#endregion


	#region Methods
	// Start is called before the first frame update
	void Start()
    {
		_toolManager = GameSettings.Instance.ToolManager;
		//Link delegates
		ToolManager.refreshToolList += RefreshList;
		unlockedToolList = _toolManager.UnlockedTools;
		currentTool_Obj = unlockedToolList[0];
    }

    // Update is called once per frame
    void Update()
    {
		ChangeTool();
		ToggleToolMode();
		if (Input.GetButtonDown("Fire1"))
		{
			_toolManager.UnlockTool("Pickaxe");
		}
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
		if (Input.GetKeyDown(KeyCode.Q))
		{
			currentTool--;
			CurrentToolWrap();
			//Call delegate
			displayNewTool(unlockedToolList[currentTool].GetComponent<Tool>());
			ShowCurrentTool();
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			currentTool++;
			CurrentToolWrap();
			//Call delegate
			displayNewTool(unlockedToolList[currentTool].GetComponent<Tool>());
			ShowCurrentTool();
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

	private void ShowCurrentTool()
	{
		Destroy(currentTool_Obj);
		GameObject newTool = Instantiate(unlockedToolList[currentTool], handPosition);
		currentTool_Obj = newTool;
	}

	/// <summary>
	/// Toggles tool mode on and off
	/// </summary>
	private void ToggleToolMode()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (toolMode)
			{
				toolMode = false;

			}
			else
			{
				toolMode = true;
			}
		}
		if (toolMode)
			currentTool_Obj.SetActive(true);
		else
			currentTool_Obj.SetActive(false);

	}

	#endregion
}
