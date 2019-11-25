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
	[SerializeField] private Image growthDisplay;
	[SerializeField] private GameObject mound;

	public Image GrowthDisplay { get { return growthDisplay; } }

	private void Start()
	{
		this.gameObject.layer = 0;
		growthDisplay.gameObject.SetActive(false);
		mainCam = Camera.main.transform.position;
		mound.SetActive(false);
	}

	/// <summary>
	/// Called when a plant is added to the plot
	/// </summary>
	public void Plant()
	{	
		isPlanted = true;
		growthDisplay.fillAmount = 0;
		mound.SetActive(true);
	}
	private void Update()
	{
		mainCam = Camera.main.transform.position;
		growthDisplay.transform.LookAt(mainCam);

		//If there is a plant planted in this plot
		if(isPlanted)
		{
			mound.SetActive(true);
			plant = gameObject.transform.GetComponentInChildren<Plant>();

			//Update visual percentage of plant growth
			if (plant.IsGrowing)
			{
				growthDisplay.gameObject.SetActive(true);
				growthDisplay.fillAmount = plant.PercentGrown;
			}
			else
			{
				growthDisplay.gameObject.SetActive(false);
			}
		
		}
		else
		{
			growthDisplay.gameObject.SetActive(false);
			mound.SetActive(false);
		}
	}

	/// <summary>
	/// Called when a plant is removed from the plot
	/// </summary>
	public void Reset()
	{
		isPlanted = false;
	}

	public void ShowProgress()
	{
		growthDisplay.gameObject.SetActive(true);
		StartCoroutine(HideProgress());
	}

	IEnumerator HideProgress()
	{
		yield return new WaitForSeconds(1f);
		growthDisplay.gameObject.SetActive(false);
	}
}
