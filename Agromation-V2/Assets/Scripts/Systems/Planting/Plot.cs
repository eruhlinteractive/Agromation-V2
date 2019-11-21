using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
	private Vector3 plantPos;
	private bool isPlanted;

	public bool IsPlanted { get => isPlanted;}

	private void Start()
	{
		plantPos = transform.GetChild(0).position;
	}

	public void Plant()
	{	
		isPlanted = true;
	}
	public void Reset()
	{
		isPlanted = false;
	}
}
