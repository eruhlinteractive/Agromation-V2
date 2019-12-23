using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResources : MonoBehaviour
{

	[SerializeField] private GameObject _programmingUI;
	public  GameObject ProgrammingUI { get { return _programmingUI; } }
	[SerializeField] private GameObject _craftingUI;
	public GameObject CraftingUI { get => _craftingUI; }


	//Singleton
	private static UIResources instance;
	public static UIResources Instance { get { return instance; } }

	

	// Start is called before the first frame update
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
        
    }
}
