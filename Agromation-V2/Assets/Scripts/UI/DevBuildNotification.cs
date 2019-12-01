using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevBuildNotification : MonoBehaviour
{
	private PlayerControlManager _pcm;
    // Start is called before the first frame update
    void Start()
    {
		_pcm = GameSettings.Instance.PlayerControlManager;
		_pcm.UnlockCursor();
	}
	void Update()
	{
		
	}

    public void Okay()
	{
		_pcm.LockCursor();
		this.gameObject.SetActive(false);
	}
}
