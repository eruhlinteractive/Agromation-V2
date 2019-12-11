using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

	[SerializeField] private GameObject arrow;
	[SerializeField] private Camera mainCam;
	[SerializeField] private Transform shootPos;
	[SerializeField] private float baseFirePower;
	[SerializeField] private float maxFirePower;
	[SerializeField] private GameObject displayArrow;
	float firePower;
	[SerializeField] float chargeTime;
	[SerializeField] float baseZoom;
	[SerializeField] float fullZoom;
	[SerializeField] RaycastHit shootHit;
	// Start is called before the first frame update
	void Start()
    {
		mainCam = Camera.main;
		baseZoom = Camera.main.fieldOfView;

		//Reset parameters
		mainCam.fieldOfView = baseZoom;
		firePower = baseFirePower;
	}

    // Update is called once per frame
    void Update()
    {
		//"Charging the shot"
		if (Input.GetButton("Fire1"))
		{
			displayArrow.SetActive(true);
			//Charge up bow 
			if (mainCam.fieldOfView > fullZoom)
			mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, fullZoom, Time.deltaTime * chargeTime);

			if(firePower < maxFirePower)
			firePower = Mathf.Lerp(firePower, maxFirePower, Time.deltaTime * chargeTime);
			
		}

		//Fire button is "Released"
		if(Input.GetButtonUp("Fire1"))
		{
			displayArrow.SetActive(false);
			mainCam.fieldOfView = baseZoom;
			Fire();
		}

		Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out shootHit, 100f);
        
    }

	/// <summary>
	/// Fire an arrow
	/// </summary>
	private void Fire()
	{
		//Assist with aiming
		Vector3 targetPos;
		if (shootHit.collider != null)
		{
			targetPos = shootHit.point;
		}
		else
		{
			targetPos = mainCam.transform.position + mainCam.transform.forward * 25f;
		}
		 
		//Create/ shoot the arrow
		Rigidbody arrowShot = Instantiate(arrow, shootPos.position,Quaternion.LookRotation(mainCam.transform.forward)).GetComponent<Rigidbody>();
		arrowShot.AddForce((targetPos - arrowShot.transform.position).normalized * firePower, ForceMode.Impulse);

		//Reset parameters
		mainCam.fieldOfView = baseZoom;
		firePower = baseFirePower;
	}
}
