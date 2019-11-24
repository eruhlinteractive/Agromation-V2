using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingPortal : MonoBehaviour
{

	private PlayerStats _playerStats;

	private ParticleSystem particleEffect;
    // Start is called before the first frame update
    void Start()
    {
		_playerStats = GameSettings.Instance.PlayerStats;
		particleEffect = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Item"))
		{
			int itemValue = collision.gameObject.GetComponent<Item>().Value;
			Destroy(collision.gameObject);
			_playerStats.AddMoney(itemValue);
			particleEffect.Play();
			
		}
	}
}
