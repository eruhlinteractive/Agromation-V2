using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGrowable
{
	void StartGrowing();
	void FullGrown();

}

interface IPlacable
{
	 GameObject PlacedObject { get; }
}

