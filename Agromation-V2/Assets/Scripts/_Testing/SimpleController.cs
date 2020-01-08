using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{

	[SerializeField] Transform head;
	[SerializeField] Transform target;
	[SerializeField] GameObject projectile;
	[SerializeField] Transform firePos;
	[SerializeField] float coreHeight;
	Vector3 StartDirection;
	Quaternion StartRotation;

	[SerializeField] float moveSpeed;
	[SerializeField] float rotationSpeed;



	private void Start()
	{
		StartDirection = new Vector3(target.transform.position.x, head.transform.position.y, target.transform.position.z) - head.transform.position;
		StartRotation = head.transform.rotation;
		
	}
	// Update is called once per frame
	void Update()
	{
		RaycastHit groundHit;

		Physics.Raycast(transform.position+ Vector3.up * 2f, Vector3.down, out groundHit, 8f);

		Debug.DrawRay(transform.position + Vector3.up * 2f, Vector3.down * 8f);
		//Set the height of the core 
		transform.position = Vector3.Lerp(transform.position, 
			new Vector3(transform.position.x, groundHit.point.y + coreHeight, transform.position.z),
			Time.deltaTime * 2f);

		//Set head Direction
		Vector3 lookPos = target.position - head.transform.position;
		lookPos = new Vector3(lookPos.x, 0, lookPos.z);
		head.transform.rotation = Quaternion.LookRotation(lookPos) * Quaternion.Euler(new Vector3(-90,180,0));


		if(Input.GetButtonDown("Jump"))
		{
			Fire(firePos.position);
		}




	}

	private void FixedUpdate()
	{
		//Apply Input
		if (Input.GetAxisRaw("Vertical") != 0)
		{
			transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * moveSpeed * Input.GetAxis("Vertical"), Time.fixedDeltaTime * moveSpeed);
		}

		if (Input.GetAxis("Horizontal") != 0)
		{
			transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * Input.GetAxis("Horizontal"));
		}
	}

	void Fire(Vector3 firePos)
	{
		GameObject proj = Instantiate(projectile, firePos, Quaternion.identity);
		proj.GetComponent<Rigidbody>().AddForce((target.position - firePos).normalized * 25f,ForceMode.Impulse);


	}
}
