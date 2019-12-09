using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationLinker : MonoBehaviour
{

	private PlayerLookRayCast _playerLookRayCast;
	private PlayerInventory _playerInventory;

	private DroneStation stationToLink = null;
	private DronePad padToLink = null;
	private LineRenderer line;
	[SerializeField] Transform toolEnd;


	// Start is called before the first frame update
	void Start()
    {
		line = GetComponent<LineRenderer>();
		_playerLookRayCast = PlayerLookRayCast.Instance;
		_playerInventory = GameSettings.Instance.PlayerInventory;
		line.SetPosition(0, toolEnd.position);
    }

    // Update is called once per frame
    void Update()
    {
		//Put one end of the link indicator at the end of the tool
		line.SetPosition(0, toolEnd.position);



		if (_playerLookRayCast.LookHit.collider != null)
		{
			//Looking at potential station
			if (_playerLookRayCast.LookHit.collider.gameObject.GetComponent<DroneStation>() != null)
			{
				if (Input.GetButtonDown("Fire1"))
				{
					stationToLink = _playerLookRayCast.LookHit.collider.GetComponent<DroneStation>();
					line.SetPosition(1, stationToLink.transform.position + Vector3.up * 2);
					line.enabled = true;
				}
			}
			//Looking at potential Pad
			if (Input.GetButtonDown("Fire1"))
			{
				if (_playerLookRayCast.LookHit.collider.GetComponent<DronePad>() != null)
				{
					padToLink = _playerLookRayCast.LookHit.collider.GetComponent<DronePad>();
					line.SetPosition(1, padToLink.transform.position);
					line.enabled = true;

				}
			}
		}

		//If both a pad and a station are selected
		if (padToLink != null && stationToLink != null)
		{
			line.SetPosition(1, padToLink.transform.position);
			line.SetPosition(0, stationToLink.transform.position + Vector3.up * 2);
			//Debug.Log("Ready to Link!");

			padToLink.LinkStation(stationToLink);
			stationToLink.LinkDronePad(padToLink);

		}

    }

	
}
