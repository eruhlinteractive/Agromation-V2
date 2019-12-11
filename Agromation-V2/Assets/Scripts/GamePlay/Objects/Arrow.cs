using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	private Vector3 latestVel;
	private void LateUpdate()
	{
		if(gameObject.GetComponent<Rigidbody>()!= null)
		{
			//Update rotation to match the velocity
			if(gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
			{
				Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
				transform.rotation = Quaternion.LookRotation(v);
				latestVel = v;
			}
		}


	}

	private void OnCollisionEnter(Collision collision)
	{
		//"Freeze" the arrow
		transform.rotation = Quaternion.LookRotation(latestVel);
		transform.parent = collision.collider.gameObject.transform;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Collider>().enabled = false;
		Destroy(gameObject, 10f);
	}
}
