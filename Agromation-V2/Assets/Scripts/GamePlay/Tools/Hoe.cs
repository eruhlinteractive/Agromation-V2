using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : MonoBehaviour
{
	[SerializeField] PlotManager _plotManager;
	[SerializeField] Grid _grid;
	[SerializeField] GameObject groundPlot;
	[SerializeField] Transform headPos;
	GameObject trackingPoint;
	RaycastHit groundCheck;

	// Start is called before the first frame update
	void Start()
    {
        _plotManager = PlotManager.Instance;
		_grid = Grid.Instance;
		headPos = GetComponentInParent<Camera>().transform;
	}

    // Update is called once per frame
    void Update()
    {
		if (Physics.Raycast(headPos.transform.position, headPos.transform.forward, out groundCheck, 5f))
		{
			Debug.DrawRay(headPos.transform.position, headPos.transform.forward * 5f, Color.blue);
			if (Input.GetButtonDown("Fire1"))
			{
				if (groundCheck.collider.CompareTag("Ground"))
				{
					Vector3 pos = _grid.GetNearestPointOnGrid(groundCheck.point);
					if (_plotManager.OpenSpot(pos))
					{
						GameObject newPlot = Instantiate(groundPlot, pos, Quaternion.identity);
						newPlot.GetComponent<Rigidbody>().isKinematic = true;
						_plotManager.AddPlot(newPlot.transform.position, newPlot);
					}

				}
			}
		}
		
    }
}
