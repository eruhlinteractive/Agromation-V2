using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Item, IGrowable
{
	[SerializeField] private bool isGrowing;
	[SerializeField] private bool fullyGrown;
	[SerializeField]float growtime;
	float growtimeElapsed;

	public bool FullyGrown { get => fullyGrown;}


	// Start is called before the first frame update
	void Start()
    {
		
		if(transform.parent != null)
		{
			StartGrowing();
			GetComponent<Collider>().enabled = false;
			GetComponent<Rigidbody>().isKinematic = true;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (isGrowing)
		{
			growtimeElapsed += Time.deltaTime;

			if(growtimeElapsed >= growtime)
			{
				FullGrown();
			}
		}
        
    }

	/// <summary>
	/// Start the growing timer for this plant
	/// </summary>
	public void StartGrowing()
	{
		growtimeElapsed = 0;
		isGrowing = true;
	}

	/// <summary>
	/// Trigger Events when its fully grown
	/// </summary>
	public void FullGrown()
	{
		isGrowing = false;
		fullyGrown = true;
		//Debug.Log("PlantFullyGrown");

		//Reset plot and unparent
		transform.parent.GetComponent<Plot>().Reset();
		transform.parent = null;
		
		//Make "Pickupable"
		transform.position += Vector3.up;
		gameObject.GetComponent<Collider>().enabled = true;
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.tag = "Item";
	}
}
