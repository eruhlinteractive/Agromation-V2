using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : MonoBehaviour
{
	[SerializeField] PlotManager _plotManager = null;
	[SerializeField] Grid _grid = null;
	PlayerLookRayCast _playerLookRayCast;
	[SerializeField] GameObject groundPlot = null;

	// Start is called before the first frame update
	void Start()
    {
        _plotManager = PlotManager.Instance;
		_grid = Grid.Instance;
		_playerLookRayCast = PlayerLookRayCast.Instance;
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Fire1"))
		{
			if (_playerLookRayCast.LookHit.collider.CompareTag("Ground"))
			{
				//Get the nearest point on the grid
				Vector3 pos = _grid.GetNearestPointOnGrid(_playerLookRayCast.LookHit.point);
				if (_plotManager.OpenSpot(pos))
				{
					//Create the plot and add it to the plot managers index
					GameObject newPlot = Instantiate(groundPlot, pos, Quaternion.identity);
					_plotManager.AddPlot(newPlot.transform.position, newPlot);
				}

			}
		}
    }
}
