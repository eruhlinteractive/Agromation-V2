using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStation : MonoBehaviour
{

	[SerializeField] private DroneControl drone;
	[SerializeField] private string instructions;
	[SerializeField] private PlayerControlManager _playerControlManager;
	[SerializeField] private GameObject programmingUI;

	public delegate void OpenProgrammingWindow(DroneStation station);
	public static OpenProgrammingWindow openConsole;

	public string StoredInstructions { get => instructions;}

	private void Start()
	{
		_playerControlManager = GameSettings.Instance.PlayerControlManager;
		programmingUI = UIResources.Instance.ProgrammingUI;
	}


	/// <summary>
	/// Locks player movement and opens programming menu
	/// </summary>
	public void OpenConsole()
	{
		programmingUI.SetActive(true);
		_playerControlManager.UnlockCursor();
		openConsole(this);
	}

	/// <summary>
	/// Unlocks player movement when the programming menu is closed
	/// </summary>
	public void CloseConsole()
	{
		_playerControlManager.LockCursor();
	}

	/// <summary>
	/// Sets the instruction of the strings in the station
	/// </summary>
	/// <param name="instructionString">The raw instructions from the UI window</param>
	public void TransferInstructions(string instructionString)
	{
		instructions = instructionString;
		Debug.Log("Got instructions:" + instructionString);

		if(drone != null)
		{
			//Apply new commands to the drone
			drone.SetCommands(instructions);
		}
	}

	/// <summary>
	/// Links the drone to the current control station
	/// </summary>
	/// <param name="droneToAdd">The drone to link to this station</param>
	public void AddDrone(DroneControl droneToAdd)
	{
		if(drone == null)
		{
			drone = droneToAdd;
		}
	}
}
