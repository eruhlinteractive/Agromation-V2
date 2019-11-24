using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonModule : DayNight_ModuleBase
{
	//From tutorial https://www.youtube.com/watch?v=dkrPcCv-lcY
	[SerializeField] private Light moon;
	[SerializeField] private Gradient moonColor;
	[SerializeField] private float baseMoonIntensity;
	

	public override void UpdateModule(float intensity)
	{
		moon.color = moonColor.Evaluate(1 - intensity);
		moon.intensity = (1 - intensity) * baseMoonIntensity + 0.05f;
	}
}
