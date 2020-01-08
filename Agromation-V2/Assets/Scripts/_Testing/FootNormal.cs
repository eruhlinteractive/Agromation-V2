using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootNormal : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
		RaycastHit normHit;

		Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out normHit, 15f);

		//if(normHit.collider != null)
		//{
		//	transform.up = normHit.normal;
		//}

		transform.position = new Vector3(transform.position.x, normHit.point.y + 0.2f, transform.position.z);
    }
}
