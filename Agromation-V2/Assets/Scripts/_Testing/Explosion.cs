using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public MeshRenderer sphere;
	public GameObject particle;





	private void OnCollisionEnter(Collision collision)
	{
		sphere.enabled = false;
		this.GetComponent<Rigidbody>().isKinematic = true;
		particle.SetActive(true);
		Destroy(this.gameObject, 1.5f);
	}

}
