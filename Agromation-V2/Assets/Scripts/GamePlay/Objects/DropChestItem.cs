using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropChestItem : MonoBehaviour
{

	private Vector3 velocity = Vector3.up;
	private Rigidbody rb;
	private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
		startPos = this.transform.position;
		velocity *= Random.Range(4f, 6f);   //Random Upward velocity
		velocity += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

		//Set rb properties
		rb = this.GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		rb.position += velocity * Time.deltaTime; //Update item position

		//Give item a slight velocity through the air
		Quaternion deltaRot = Quaternion.Euler(new Vector3(
			Random.RandomRange(-150f, 150f),
			Random.RandomRange(150f, 250f),
			Random.RandomRange(-150f, 150f))
			* Time.deltaTime);

		rb.MoveRotation(rb.rotation * deltaRot);

		//Limit downward velocity
		if(velocity.y < -4f)
		{
			velocity.y = -4f;
		}
		else
		{
			velocity -= Vector3.up * 5 * Time.deltaTime; //5 is half the default gravitational acceleration in unity
		}

		//If the object is close to its original position and moving downward
		//Let the Unity physics engine take over and disable the script
		if(Mathf.Abs(rb.position.y - startPos.y) > .25f && velocity.y < 0f)
		{
			rb.useGravity = true;
			rb.isKinematic = false;
			rb.velocity = velocity;
			this.enabled = false;	//Disable this script
		}

    }
}
