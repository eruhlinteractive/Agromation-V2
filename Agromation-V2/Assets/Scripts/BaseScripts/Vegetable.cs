using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : Plant, IGrowable
{
	Vector3 fullGrownScale;


	// Start is called before the first frame update
	void Start()
	{

		if (transform.parent != null)
		{
			fullGrownScale = transform.localScale;

			//Quarter the scale
			transform.localScale = Vector3.zero;
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
			growTimeElapsed += Time.deltaTime;
			transform.localScale = Vector3.Lerp(transform.localScale, fullGrownScale, growTimeElapsed * Time.deltaTime / growTime);
			percentGrown = growTimeElapsed / growTime;
			if (growTimeElapsed >= growTime)
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
		growTimeElapsed = 0;
		isGrowing = true;
	}

	/// <summary>
	/// Trigger Events when its fully grown
	/// </summary>
	public void FullGrown()
	{
		isGrowing = false;
		fullGrown = true;
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
