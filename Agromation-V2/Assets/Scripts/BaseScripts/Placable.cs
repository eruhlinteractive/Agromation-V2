using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : Item
{
	[SerializeField] private GameObject placableObject;

	public GameObject PlacableObject { get => placableObject;}
}
