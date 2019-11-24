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
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		EnablePlayerMovement();
	}

	public void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		DisablePlayerMovement();
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
