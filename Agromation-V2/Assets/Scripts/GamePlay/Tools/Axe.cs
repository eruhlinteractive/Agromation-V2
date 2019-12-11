using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
	PlayerLookRayCast _playerLookRaycast;
    // Start is called before the first frame update
    void Start()
    {
		_playerLookRaycast = PlayerLookRayCast.Instance;
	}

    // Update is called once per frame
    void Update()
    {
		//Type check 
		if (Input.GetButtonDown("Fire1"))
		{
			if(_playerLookRaycast.LookHit.collider != null)
			{
				//Is the player looking at a resource
				if(_playerLookRaycast.LookHit.collider.GetComponent<Resource>() != null)
				{
					//Is it wood?
					if(_playerLookRaycast.LookHit.collider.GetComponent<Resource>().Type == ResourceType.Wood)
					{
						//Deal damage
						_playerLookRaycast.LookHit.collider.GetComponent<Resource>().Hit();
					}
				}
			}
		}
        
    }
}
