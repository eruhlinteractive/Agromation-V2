using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftablePlacable : CraftableObject, IPlacable
{

	[SerializeField] private GameObject placedObject;

	public GameObject PlacedObject { get => placedObject; }
}
