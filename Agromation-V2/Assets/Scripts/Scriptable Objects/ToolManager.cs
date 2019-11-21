using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tool Manager")]
public class ToolManager : ScriptableObject
{
	[SerializeField] private List<GameObject> allTools;
	private Dictionary<string, GameObject> toolIndex = new Dictionary<string, GameObject>(); //Format <Name,Prefab reference>
	[SerializeField] private List<GameObject> unlockedTools;

	public List<GameObject> UnlockedTools { get => unlockedTools;}

	//Tool List
	public delegate void NewToolUnlock();
	public static NewToolUnlock refreshToolList;


	// Start is called before the first frame update
	public void Initialize()
    {
		//Populate tool dictionary
		for (int i = 0; i < allTools.Count; i++)
		{
			toolIndex.Add(allTools[i].GetComponent<Tool>().ToolName, allTools[i]);
		}
    }

	/// <summary>
	/// Has the tool been unlocked already?
	/// </summary>
	/// <param name="toolName">The tool name to check</param>
	/// <returns>True if the tool has been unlocked already</returns>
	private bool Unlocked(string toolName)
	{
		if (unlockedTools.Contains(toolIndex[toolName]))
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	public void UnlockTool(string toolName)
	{
		if (toolIndex.ContainsKey(toolName))
		{
			//If its NOT unlocked already
			if (!Unlocked(toolName))
			{
				unlockedTools.Add(toolIndex[toolName]);
				refreshToolList();
			}
		}
	}
}
