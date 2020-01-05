using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

	private PlayerControlManager _playerControlManager;
	bool isPaused = false;
	[SerializeField] GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
		_playerControlManager = GameSettings.Instance.PlayerControlManager;
    }

	private void Update()
	{
		if(!isPaused)
		if (Input.GetButtonDown("Cancel"))
		{
			PauseGame();
		}
	}


	public void Quit()
	{
		Application.Quit();
	}

	private void PauseGame()
	{
		Time.timeScale = 0;
		_playerControlManager.UnlockCursor();
		pauseMenu.SetActive(true);
	}


	public void UnPauseGame()
	{
		Time.timeScale = 1;
		pauseMenu.SetActive(false);
		_playerControlManager.LockCursor();
	}



}
