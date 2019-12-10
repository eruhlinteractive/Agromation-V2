using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePad : MonoBehaviour
{
	DroneStation linkedStation;
	DroneControl linkedDrone = null;

	public DroneControl LinkedDrone { get { return linkedDrone; } }

	public void LinkStation(DroneStation stationToLink)
	{
		linkedStation = stationToLink;
	}

	public void LinkDrone(DroneControl droneToLink)
	{
		linkedDrone = droneToLink;
	}

	public void SetDroneCommands(string commands)
	{
		if(linkedDrone != null)
		{
			linkedDrone.SetCommands(commands);
		}
	}
}
