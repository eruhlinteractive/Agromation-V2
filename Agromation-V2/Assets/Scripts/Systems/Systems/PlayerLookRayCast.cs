using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookRayCast : MonoBehaviour
{
	[SerializeField] private float distance = 0;
	private RaycastHit lookHit;

	public RaycastHit LookHit { get { return lookHit; } }

	private static PlayerLookRayCast instance = null;
	public static PlayerLookRayCast Instance { get { return instance; } }

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

    // Update is called once per frame
    void Update()
    {
		//Do the Raycast and give info to lookHit
		Physics.Raycast(transform.position, transform.forward, out lookHit, distance);
		Debug.DrawRay(transform.position, transform.forward * distance, Color.cyan);
	}
}

