using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushPlant : Item,IGrowable
{

	[SerializeField] private List<GameObject> vegetables;
	[SerializeField] private List<Transform> growPositions;
	 private int amountOfVegetables;
	[SerializeField] private int growLimit;
	private int timesGrown = 0;

	[SerializeField] private float growTime;
	[SerializeField] private float growTimeElapsed;
	[SerializeField] private int vegetableId;

	private bool isFullyGrown = false;
	private bool isGrowing = false;

	[SerializeField] private ItemManager _itemManager;

	// Start is called before the first frame update
	void Start()
    {
		_itemManager = GameSettings.Instance.ItemManager;
		amountOfVegetables = growPositions.Count;

		//Add vegetables to the positions
		if(vegetables.Count == 0)
		{
			StartGrowingVegetables();
		}
    }

	/// <summary>
	/// Populate vegetable list with new vegetables and start growing
	/// </summary>
	public void StartGrowingVegetables()
	{
		growTimeElapsed = 0;
		isFullyGrown = false;
		//Create and add vegetables to the list
		for (int i = 0; i < amountOfVegetables; i++)
		{
			GameObject newVegetable = Instantiate(_itemManager.GetItem(vegetableId), growPositions[i]);

			//Make sure the vegetable rigidbody is disabled
			//if (newVegetable.GetComponent<Rigidbody>() != null)
			//{
				newVegetable.GetComponent<Rigidbody>().isKinematic = true;
				newVegetable.GetComponent<Collider>().enabled = false;
			//}
				vegetables.Add(newVegetable);
		}
		//Start the growing timer
		StartGrowing();
	}

    // Update is called once per frame
    void Update()
    {
		if (isGrowing)
		{
			growTimeElapsed += Time.deltaTime;

			if (growTimeElapsed >= growTime)
			{
				FullGrown();
			}
		}

	}


	public void FullGrown()
	{
		isFullyGrown = true;
		isGrowing = false;

		//Remove fully grown vegetables from plant
		for (int i = 0; i < vegetables.Count; i++)
		{
			if(vegetables[i].GetComponent<Rigidbody>() != null)
			{
				vegetables[i].GetComponent<Rigidbody>().isKinematic = false;
			}
			vegetables[i].GetComponent<Collider>().enabled = true;
			//Unset vegetables transform
			vegetables[i].transform.parent = null;

			//Remove "fully grown" vegetable from the list of vegetables
			vegetables.Remove(vegetables[i]);
			i--;
		}
	

		//Check if it has reached the max number of growth iterations
		timesGrown++;
		if(timesGrown >= growLimit)
		{
			if(transform.parent!= null)
			{
				transform.parent.GetComponent<Plot>().Reset();
			}
			GameObject.Destroy(this.gameObject);
		}
		//Otherwise, start another cycle
		else
		{
			StartGrowingVegetables();
		}

	}

	/// <summary>
	/// Start the growing timer
	/// </summary>
	public void StartGrowing()
	{
		isGrowing = true;
		isFullyGrown = false;
	}
}
