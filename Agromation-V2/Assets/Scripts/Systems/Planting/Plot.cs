using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
	//private Vector3 plantPos;
	private bool isPlanted = false;

	public bool IsPlanted { get => isPlanted;}

	private void Start()
	{
		this.gameObject.layer = 0;
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
