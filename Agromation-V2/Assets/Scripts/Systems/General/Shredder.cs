using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Destroy(collision.collider.gameObject);
	}
}
