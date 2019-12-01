using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePlacer : MonoBehaviour
{

	private PlayerLookRayCast _playerLookRayCast;
	private PlayerInventory _playerInventory;
	private ItemManager _itemIndex;
	[SerializeField] private int baseDroneId;
	[SerializeField] private int baseDronePlacementItemId;

	// Start is called before the first frame update
	void Start()
    {
		_playerLookRayCast = PlayerLookRayCast.Instance;
		_playerInventory = GameSettings.Instance.PlayerInventory;
		_itemIndex = GameSettings.Instance.ItemManager;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Fire2"))
		{
			if(_playerInventory.AmountInInventory(baseDroneId) > 0)
			{
				if (_playerLookRayCast.LookHit.collider.CompareTag("DronePad"))
				{


					///////-----------------------------------------------------HAVE NOT TESTED YET!!!!!----------------------------------------------------------------
					GameObject dronePad = _playerLookRayCast.LookHit.collider.gameObject;
					Instantiate(_itemIndex.GetItem(baseDroneId), Grid.Instance.GetNearestPointOnGrid(_playerLookRayCast.LookHit.point), dronePad.transform.rotation);
					_playerInventory.RemoveFromInventory(baseDronePlacementItemId);
				}	
			}
		}
    }
}
