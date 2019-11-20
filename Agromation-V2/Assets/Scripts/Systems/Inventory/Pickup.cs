using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	[SerializeField] private Inventory _inv;




	// Start is called before the first frame update
	void Awake()
    {
		_inv = gameObject.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
	
    }

	private void FixedUpdate()
	{
		
		
	}
}
