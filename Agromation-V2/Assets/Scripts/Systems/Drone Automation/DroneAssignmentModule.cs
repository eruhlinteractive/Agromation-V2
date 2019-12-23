using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DroneAssignmentModule : MonoBehaviour
{
	[SerializeField] private GameObject groundDroneObject;
	[SerializeField] private GameObject flyingDroneObject;

	public GameObject FlyingDroneObject { get => flyingDroneObject; }
	public GameObject GroundDroneObject { get => groundDroneObject; }


}
