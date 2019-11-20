using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemIndex")]
public class AllGameItems : ScriptableObject
{

	public List<GameObject> items;
	//public List<int> itemIds;
	[SerializeField]

	public List<GameObject> Items { get { return items; } }
	//public List<int> ItemIds { get { return itemIds; } }
}
