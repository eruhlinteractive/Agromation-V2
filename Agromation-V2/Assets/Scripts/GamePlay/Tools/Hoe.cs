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
		if (_playerLookRayCast.LookHit.collider != null)    //First check if null
		{
			//Create a new plot
			if (_playerLookRayCast.LookHit.collider.CompareTag("Ground"))
			{
				//Get the nearest point on the grid
				Vector3 pos = _grid.GetNearestPointOnGrid(_playerLookRayCast.LookHit.point);
				PlaceNewPlot(pos);
			}

			//If player is already looking at a plot
			if (_playerLookRayCast.LookHit.collider.CompareTag("Plot"))
			{
				Vector3 pos = _grid.GetNearestPointOnGrid(_playerLookRayCast.LookHit.point);
				PlotInteraction(pos);
			}
		}
	}

	/// <summary>
	/// Defines the interaction of the hoe with an existing ground plot
	/// </summary>
	/// <param name="pos">The position of the plot to interact with</param>
	private void PlotInteraction(Vector3 pos)
	{
		if (Input.GetButtonDown("Fire2"))
		{
			if (_plotManager.GetPlot(pos) != null)
			{
				GameObject plot = _plotManager.GetPlot(pos);

				//If theres something planted, destroy it
				if (plot.transform.childCount == 1)
				{
					Destroy(plot.transform.GetChild(0).gameObject);	//Destroy plant object
					plot.GetComponent<Plot>().Reset();  //Reset the current plot
				}
				//Otherwise destroy the whole plot
				else
				{
					_plotManager.RemovePlot(pos);

				}
			}
		}
	}

	/// <summary>
	/// Creates a new planting plot and add it to the plotIndex
	/// </summary>
	/// <param name="positionToPlacePlot">The grid position to place the plot at</param>
	private void PlaceNewPlot(Vector3 positionToPlacePlot)
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (_plotManager.OpenSpot(positionToPlacePlot))
			{
				//Create the plot and add it to the plot managers index
				GameObject newPlot = Instantiate(groundPlot, positionToPlacePlot, Quaternion.identity);
				_plotManager.AddPlot(newPlot.transform.position, newPlot);
			}
		}
	}
}
