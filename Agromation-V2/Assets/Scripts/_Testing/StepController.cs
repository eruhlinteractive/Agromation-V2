using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepController : MonoBehaviour
{
	[Header("Group 1 Legs")]
	[SerializeField] LegStepper legOne;
	[SerializeField] LegStepper legTwo;
	[SerializeField] LegStepper legThree;


	[Header("Group 2 Legs")]
	[SerializeField] LegStepper legFour;
	[SerializeField] LegStepper legFive;
	[SerializeField] LegStepper legSix;

	void OnEnable()
	{
		StartCoroutine(LegUpdate());
	}

	//Update Legs
	IEnumerator LegUpdate()
	{

		while (true)
		{
			//Stay in loop if one leg is moving
			do
			{
				legOne.TryMove();
				legTwo.TryMove();
				legThree.TryMove();
				yield return null;
			} while (legOne.Moving || legTwo.Moving || legThree.Moving);

			do
			{
				legFour.TryMove();
				legFive.TryMove();
				legSix.TryMove();
				yield return null;
			} while (legFour.Moving || legFive.Moving || legSix.Moving);

		}


	}
}
