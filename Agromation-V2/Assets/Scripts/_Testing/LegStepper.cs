using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegStepper : MonoBehaviour
{

	[SerializeField] Transform homePos;
	[SerializeField] float distToStep;
	[SerializeField] bool moving;
	[SerializeField] float moveDuration;


	public bool Moving { get { return moving; } }


	private void OnEnable()
	{
		moving = false;
	}

	// Update is called once per frame
	void Update()
    {
		//TryMove();
    }

	/// <summary>
	/// Determine if end Position needs to be moved towards home position
	/// </summary>
	public void TryMove()
	{
		if (Vector3.Distance(transform.position, homePos.position) > distToStep)
		{
			if (isActiveAndEnabled)
			{
				StartCoroutine(MoveToHomePosition());
			}
			
		}
	}

	/// <summary>
	/// Move the endpoint to the home position
	/// </summary>
	/// <returns></returns>
	IEnumerator MoveToHomePosition()
	{

			moving = true;

			Vector3 startPoint = transform.position;

			// Directional vector from the foot to the home position
			Vector3 towardHome = (homePos.position - transform.position);
			// Total distnace to overshoot by   
			float overshootDistance = distToStep * 0.5f;
			Vector3 overshootVector = towardHome * overshootDistance;
			// Since we don't ground the point in this simplified implementation,
			// we restrict the overshoot vector to be level with the ground
			// by projecting it on the world XZ plane.
			overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

			// Apply the overshoot
			Vector3 endPoint = homePos.position + overshootVector;

			// We want to pass through the center point
			Vector3 centerPoint = (startPoint + endPoint) / 2;
			// But also lift off, so we move it up by half the step distance (arbitrarily)
			centerPoint += homePos.up * Vector3.Distance(startPoint, endPoint) / 2f;
			float timeElapsed = 0;
			do
			{
				timeElapsed += Time.deltaTime;
				float normalizedTime = timeElapsed / moveDuration;

				// Quadratic bezier curve
				transform.position =
				  Vector3.Lerp(
					Vector3.Lerp(startPoint, centerPoint, normalizedTime),
					Vector3.Lerp(centerPoint, endPoint, normalizedTime),
					normalizedTime
				  );

				yield return null;


			}
		while (timeElapsed < moveDuration);

		// Done moving
		moving = false;
	}


	public void MoveToRest()
	{
		StartCoroutine(MoveToHomePosition());
	}
}

