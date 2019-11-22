using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleColor : MonoBehaviour
{


	private PlayerLookRayCast _playerLookRayCast;
	private Image reticleImage;
    // Start is called before the first frame update
    void Start()
    {
		reticleImage = gameObject.GetComponent<Image>();
		_playerLookRayCast = PlayerLookRayCast.Instance;
	}

    // Update is called once per frame
    void Update()
    {
		reticleImage.color = Color.red;
		if(_playerLookRayCast.LookHit.collider != null)
		if (_playerLookRayCast.LookHit.collider.gameObject.CompareTag("Item"))
		{
			reticleImage.color = Color.green;
		}
			
        
    }
}
