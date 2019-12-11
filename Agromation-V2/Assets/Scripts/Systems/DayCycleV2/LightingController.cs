using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class LightingController : MonoBehaviour
{ //Scene References
	[SerializeField] private Light sunLight;
	[SerializeField] private Light moonLight;
	[SerializeField] private LightingPreset preset;
	[SerializeField] private float timeScale;

	//Variables
	[SerializeField, Range(0, 24)] private float TimeOfDay;


	private void Update()
	{
		if (preset == null)
			return;

		if (Application.isPlaying)
		{
			//(Replace with a reference to the game time)
			TimeOfDay += Time.deltaTime/timeScale;
			TimeOfDay %= 24; //Modulus to ensure always between 0-24
			UpdateLighting(TimeOfDay / 24f);
		}
		else
		{
			UpdateLighting(TimeOfDay / 24f);
		}
	}


	private void UpdateLighting(float timePercent)
	{
		//Set ambient and fog
		RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
		RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

		//If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
		if (sunLight != null)
		{
			sunLight.color = preset.sunColor.Evaluate(timePercent);
			sunLight.intensity = preset.sunIntensity.Evaluate(timePercent);

			moonLight.intensity = (1 - sunLight.intensity) + preset.moonBaseIntenstity;
			moonLight.color = preset.moonColor.Evaluate(1-timePercent);
			transform.rotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
		}

	}

	//Try to find a directional light to use if we haven't set one
	private void OnValidate()
	{
		if (sunLight != null)
			return;

		//Search for lighting tab sun
		if (RenderSettings.sun != null)
		{
			sunLight = RenderSettings.sun;
		}
		//Search scene for light that fits criteria (directional)
		else
		{
			Light[] lights = GameObject.FindObjectsOfType<Light>();
			foreach (Light light in lights)
			{
				if (light.type == LightType.Directional)
				{
					sunLight = light;
					return;
				}
			}
		}
	}
}
