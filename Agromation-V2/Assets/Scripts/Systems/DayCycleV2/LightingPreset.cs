using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="Day Cycle Lighting Preset", menuName="Scriptable Objects/Lighting Preset")]
public class LightingPreset : ScriptableObject
{
	public Gradient ambientColor;
	public Gradient sunColor;
	public Gradient moonColor;
	public Gradient fogColor;
	public AnimationCurve sunIntensity;
	public float moonBaseIntenstity;

}
