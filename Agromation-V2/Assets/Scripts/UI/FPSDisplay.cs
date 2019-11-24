using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
	private Text display;
	float deltaTime = 0.0f;
	// Start is called before the first frame update
	void Start()
    {
		display = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		display.text = "FPS:" + (1.0f /deltaTime).ToString();
	}
}
