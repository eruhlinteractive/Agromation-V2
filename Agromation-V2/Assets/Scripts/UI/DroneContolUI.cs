using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroneContolUI : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_InputField instructions;
	[SerializeField] private DroneStation connectedStation;

	// Start is called before the first frame update
	void Awake()
	{
		//Link the delegates
		DroneStation.openConsole += SetStation;
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Get stored data instructions for the currently connected console
	/// </summary>
	/// <param name="outputStation">The connected station (which one opened the menu</param>
	private void SetStation(DroneStation outputStation)
	{
		connectedStation = outputStation;
		instructions.text = outputStation.StoredInstructions;
	}

	/// <summary>
	/// Transfers instructions to console
	/// </summary>
	public void Save()
	{
		//Debug.Log(instructions.text);
		connectedStation.TransferInstructions(instructions.text);
	}

	/// <summary>
	/// Closes Window and  and allows player to move
	/// </summary>
	public void Close()
	{
		connectedStation.CloseConsole();
		connectedStation = null;
		this.gameObject.SetActive(false);
	}
}
