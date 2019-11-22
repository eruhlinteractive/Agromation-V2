using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNameDisplay : MonoBehaviour
{
	private PlayerLookRayCast _playerLookRayCast;
	private Text nameText;
    // Start is called before the first frame update
    void Start()
    {
		nameText = gameObject.GetComponent<Text>();
		_playerLookRayCast = PlayerLookRayCast.Instance;
    }

    // Update is called once per frame
    void Update()
    {

		//Displays the text of the item the player is currently looking at
		nameText.text = "";
        if(_playerLookRayCast.LookHit.collider != null)
		{
			GameObject objectLookingAt = _playerLookRayCast.LookHit.collider.gameObject;
			if (objectLookingAt.CompareTag("Item"))
			{
				nameText.text = objectLookingAt.GetComponent<Item>().ItemName;
			}
		}
    }
}
