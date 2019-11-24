using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DayNight_ModuleBase : MonoBehaviour
{
	//From tutorial https://www.youtube.com/watch?v=dkrPcCv-lcY

	protected DayNightCycle dayNightControl;

	private void OnEnable()
	{
		dayNightControl = this.GetComponent<DayNightCycle>();
		if (dayNightControl != null)
		{
			dayNightControl.AddModule(this);
		}
	}
	private void OnDisable()
	{
		if(dayNightControl != null)
		{
			dayNightControl.RemoveModule(this);
		}
	}

	public abstract void UpdateModule(float intensity);
}
