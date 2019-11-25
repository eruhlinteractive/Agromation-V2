using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Item
{
	///Base class for growable plants 


	[Header("Inherited from 'Plant' baseClass")]
	[SerializeField] protected float growTime;
	[SerializeField] protected float growTimeElapsed;
	protected float percentGrown;
	protected bool fullGrown;
	protected bool isGrowing;

	public float GrowTime { get => growTime; }
	public float GrowTimeElapsed { get => growTimeElapsed;}
	public float PercentGrown { get => percentGrown;}
	public bool FullGrown { get => fullGrown; }
	public bool IsGrowing { get => isGrowing;}
}
