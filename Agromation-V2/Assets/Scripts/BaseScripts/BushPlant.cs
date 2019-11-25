using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushPlant : Plant,IGrowable
{

	[SerializeField] private List<GameObject> vegetables;
	[SerializeField] private List<Transform> growPositions;
	 private int amountOfVegetables;
	[SerializeField] private int growLimit;
	private int timesGrown = 0;

	private Vector3 vegFullGrownScale;
	[SerializeField] private int vegetableId;

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
		fullGrown = false;
		vegFullGrownScale = _itemManager.GetItem(vegetableId).transform.localScale;
		//Create and add vegetables to the list
		for (int i = 0; i < amountOfVegetables; i++)
		{
			GameObject newVegetable = Instantiate(_itemManager.GetItem(vegetableId), growPositions[i]);

				newVegetable.GetComponent<Rigidbody>().isKinematic = true;
				newVegetable.GetComponent<Collider>().enabled = false;
				vegetables.Add(newVegetable);

			////Quarter the scale of each vegetable
			newVegetable.transform.localScale = Vector3.zero;
		}
	
	//Start the growing timer
	StartGrowing();
	}

    // Update is called once per frame
    void Update()
    {
		if (isGrowing)
		{
			ScalePlants();

			growTimeElapsed += Time.deltaTime;
			percentGrown = growTimeElapsed / growTime;
			if (growTimeElapsed >= growTime)
			{
				FullGrown();
			}
		}

	}

	//Visually grows the plants by scaling them
	private void ScalePlants()
	{
		foreach (GameObject veg in vegetables)
		{
			veg.transform.localScale = Vector3.Lerp(veg.transform.localScale, vegFullGrownScale, growTimeElapsed * Time.deltaTime / growTime);
		}
	}
	public void FullGrown()
	{
		fullGrown = true;
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
			StartCoroutine(CyclePause());
		}

	}

	/// <summary>
	/// Start the growing timer
	/// </summary>
	public void StartGrowing()
	{
		isGrowing = true;
		fullGrown = false;
	}


	IEnumerator CyclePause()
	{
		yield return new WaitForSeconds(5);
		StartGrowingVegetables();
	}
}
