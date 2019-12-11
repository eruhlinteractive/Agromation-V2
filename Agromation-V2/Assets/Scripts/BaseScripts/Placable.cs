using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : Item, IPlacable
{
	[SerializeField] private GameObject placableObject;

	public GameObject PlacedObject { get => placableObject; }
}
