using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
	//From tutorial https://www.youtube.com/watch?v=babgYCTyw3Y

	[Tooltip("Length of the day cycle (in minutes)")]
	[SerializeField] private float cycleLength;
	[SerializeField] [Range(0f, 1f)] private float timeOfDay;
	[SerializeField] private int dayNumber = 0;
	[SerializeField] private float timeScale = 100f;
	[SerializeField] private float elapsedTime;

	[SerializeField] private AnimationCurve timeCurve;
	[SerializeField] private float timeCurveNormalization;

	[SerializeField] private bool use24HourClock = true;
	[SerializeField] private Text clockDisplay;
	[SerializeField] private Text dayCountDisplay;


	[Header("Sun Light")]
	[SerializeField]
	private Transform dailyRotation;    //Sun rotation
	[SerializeField] private Light sun;
	private float intensity;
	[SerializeField] private float sunBaseIntensity = 1f;
	[SerializeField] private float sunVariation = 1.5f;
	[SerializeField] private Gradient sunColor;


	//Delegates
	public delegate void NewDay();
	public static NewDay newDay;


	[Header("Modules")]
	private List<DayNight_ModuleBase> moduleList = new List<DayNight_ModuleBase>();

	private void Start()
	{
		NormalizeTimeCurve();
	}

	private void Update()
	{
		UpdateTimeScale();
		UpdateTime();
		UpdateClock();
		AdjustSunRotation();
		SunIntensity();
		AdjustSunColor();
		UpdateModules();
	}

	private void UpdateTimeScale()
	{   //24 / fraction of an hour we want the ingame day to be
		timeScale = 24 / (cycleLength / 60);
		timeScale *= timeCurve.Evaluate(elapsedTime / (cycleLength * 60));	//Changes timescale based on time curve
		timeScale /= timeCurveNormalization; //Keeps daytime length at target value
	}

	private void UpdateTime()
	{
		timeOfDay += Time.deltaTime * timeScale / 86400; // 86400 seconds in an irl day
		elapsedTime += Time.deltaTime;
		if (timeOfDay > 1)  //New Day!
		{
			elapsedTime = 0;
			dayNumber++;
			timeOfDay -= 1;
			dayCountDisplay.text = "Day: " + dayNumber;
			//Call newDay Delegate
			newDay();
		}
	}

	private void NormalizeTimeCurve()
	{
		float stepSize = 0.01f;
		int numberOfSteps = Mathf.FloorToInt(1f / stepSize);
		float curveTotal = 0;

		for (int i = 0; i < numberOfSteps; i++)
		{
			curveTotal += timeCurve.Evaluate(i * stepSize);
		}

		timeCurveNormalization = curveTotal / numberOfSteps;

	}
	
	/// <summary>
	/// Updates the UI clock display
	/// </summary>
	private void UpdateClock()
	{
		float time = elapsedTime / (cycleLength * 60);
		float hour = Mathf.FloorToInt(time * 24);
		float minutes = Mathf.FloorToInt(((time * 24) - hour) * 60);

		string hoursString;
		string minutesString;

		if (!use24HourClock && hour > 12)
			hour -= 12;

		//Format hour display
		if(hour < 10)
		{
			hoursString = "0" + hour;
		}
		else
		{
			hoursString = hour.ToString();
		}
		//Format minute display
		if(minutes < 10)
		{
			minutesString = "0" + minutes;
		}
		else
		{
			minutesString = minutes.ToString();
		}

		if(use24HourClock)
		{
			//Update Display
			clockDisplay.text = hoursString + ":" + minutesString;
		}
		else if(time > .5f)
		{
			clockDisplay.text = hoursString + ":" + minutesString + " pm";
		}
		else
		{
			clockDisplay.text = hoursString + ":" + minutesString + " am";
		}
		
		//Debug.Log("Time :" + hour.ToString() + ":" + minutes.ToString());
	}

	/// <summary>
	/// Rotates the sun daily
	/// </summary>
	private void AdjustSunRotation()
	{
		float sunAngle = timeOfDay * 360;
		dailyRotation.transform.localEulerAngles = new Vector3(0f, 0f, sunAngle);
	}

	/// <summary>
	/// Sets the sun light's intesity
	/// </summary>
	private void SunIntensity()
	{
		intensity = Vector3.Dot(sun.transform.forward, Vector3.down);	//Calculates dot product of sun's transform.forward and world down
		intensity = Mathf.Clamp01(intensity);
		sun.intensity = intensity * sunVariation + sunBaseIntensity;
	}

	/// <summary>
	/// Sets color of sun light to a value from the gradient
	/// </summary>
	private void AdjustSunColor()
	{
		sun.color = sunColor.Evaluate(intensity);
	}

	public void AddModule(DayNight_ModuleBase module)
	{
		moduleList.Add(module);
	}

	/// <summary>
	/// Removes a module from the module list
	/// </summary>
	/// <param name="moduleToRemove"></param>
	public void RemoveModule(DayNight_ModuleBase moduleToRemove)
	{
		if(moduleList.Contains(moduleToRemove))
		{
			moduleList.Remove(moduleToRemove);
		}
	}

	//Updates any modules added to the system
	private void UpdateModules()
	{
		foreach (DayNight_ModuleBase module in moduleList)
		{
			module.UpdateModule(intensity);
		}
	}
}
