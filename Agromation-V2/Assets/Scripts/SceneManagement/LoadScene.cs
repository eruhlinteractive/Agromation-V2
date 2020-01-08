using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

	public Transform playerPos;
    // Start is called before the first frame update
    void Awake()
    {
		SceneManager.LoadScene(1, LoadSceneMode.Additive);
		Debug.Log("Loaded Persistent Stuff");
		
    }
	private void Start()
	{
		//GameObject player = null;
		//
		//
		//while (player == null)
		//{
		//	if(GameObject.FindGameObjectWithTag("Player") != null)
		//	{
		//		player = GameObject.FindGameObjectWithTag("Player");
		//		Debug.Log(player);
		//	}
		//	
		//}
		//player.transform.position = playerPos.position;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			SceneManager.LoadScene(2);
		}
	}
}
