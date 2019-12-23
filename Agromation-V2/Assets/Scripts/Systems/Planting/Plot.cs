using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plot : MonoBehaviour
{
	//private Vector3 plantPos;
	private bool isPlanted = false;
	private Vector3 mainCam;
	public bool IsPlanted { get => isPlanted;}
	private Plant plant;
	[SerializeField] private bool isWatered = false;
	[SerializeField] private Image growthDisplay;
	[SerializeField] private GameObject mound;
	[SerializeField] private Material dry;
	[SerializeField] private Material watered;

	public Image GrowthDisplay { get { return growthDisplay; } }

	private void Start()
	{
		//this.gameObject.layer = 0;
		growthDisplay.gameObject.SetActive(false);
		mainCam = Camera.main.transform.position;
		mound.SetActive(false);
	}

	/// <summary>
	/// Called when a plant is added to the plot
	/// </summary>
	public void Plant(GameObject newPlant)
	{
		growthDisplay.fillAmount = 0;
		mound.SetActive(true);

		if (plant == null)
			plant = newPlant.GetComponent<Plant>();

		//Check if the plot is currently Watered
		if (isWatered)
		{
			//Increase plant growth speed and set as watered
			plant.GrowthSpeed += plant.GrowthSpeed / 2;
		}
		isPlanted = true;
	}
	private void Update()
	{
		mainCam = Camera.main.transform.position;
		growthDisplay.transform.LookAt(mainCam);

		//If there is a plant planted in this plot
		if(growthDisplay.gameObject.activeInHierarchy)
		{
			growthDisplay.fillAmount = plant.PercentGrown;
		}
		
		//Disable mound if not planted
		if (!isPlanted)
		{
			mound.SetActive(false);
		}
	}

	/// <summary>
	/// Called when a plant is removed from the plot
	/// </summary>
	public void Reset()
	{
		isPlanted = false;
		isWatered = false;
		this.GetComponent<Renderer>().material = dry;
	}

	/// <summary>
	/// Display growth progress of current plant(UI)
	/// </summary>
	public void ShowProgress()
	{
		//Update visual amount before showing
		growthDisplay.fillAmount = plant.PercentGrown;
		growthDisplay.gameObject.SetActive(true);
		StartCoroutine(HideProgress());
	}

	IEnumerator HideProgress()
	{
		yield return new WaitForSeconds(1f);
		growthDisplay.gameObject.SetActive(false);
	}

	/// <summary>
	/// Make the current plot "Watered"
	/// </summary>
	public void WaterPlot()
	{
		if (!isWatered)
		{
			if (isPlanted)
			{
				//Increase plant growth speed and set as watered
				plant.GrowthSpeed += plant.GrowthSpeed / 2;
			}
			isWatered = true;
			this.GetComponent<Renderer>().material = watered;
		}
	}
}
