using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerControlManager : MonoBehaviour
{

	[SerializeField] private FirstPersonController player_Controller;
	[SerializeField] private GameObject buyingMenu;
	// Start is called before the first frame update
	void Start()
	{
		
	}
	private void Awake()
	{
		LockCursor();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void StartBuying()
	{
		UnlockCursor();
		buyingMenu.SetActive(true);
	}

	public void EndBuying()
	{
		buyingMenu.SetActive(false);
		LockCursor();
	}

	public void LockCursor()
	{
		EnablePlayerMovement();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
	}

	public void UnlockCursor()
	{
		DisablePlayerMovement();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void DisablePlayerMovement()
	{
		player_Controller.enabled = false;
	}

	private void EnablePlayerMovement()
	{
		player_Controller.enabled = true;
	}


}
