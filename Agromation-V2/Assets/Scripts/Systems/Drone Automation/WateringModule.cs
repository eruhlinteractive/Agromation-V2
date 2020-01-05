using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringModule : MonoBehaviour
{
	private Grid _grid;
	private PlotManager _plotManager;
	[SerializeField] private float effectWidth;

    // Start is called before the first frame update
    void Start()
    {
		_plotManager = PlotManager.Instance;
		_grid = Grid.Instance;
    }

    // Update is called once per frame
    void Update()
    {

		Vector3 posToCheck = new Vector3(transform.position.x, 0, transform.position.z);
		for (int i = (int)(-1 * effectWidth); i < effectWidth +1; i++)
		{
			//Debug.Log(_plotManager.PlantingPlots.ContainsKey(_grid.GetNearestPointOnGrid(posToCheck + transform.right * (i * _grid.Size))));
			//If its in the planting plot list
			if(_plotManager.PlantingPlots.ContainsKey(_grid.GetNearestPointOnGrid(posToCheck + transform.right * (i * _grid.Size))))
			{
				_plotManager.PlantingPlots[_grid.GetNearestPointOnGrid(posToCheck + transform.right * (i * _grid.Size))].GetComponent<Plot>().WaterPlot();
				//Debug.Log("Plot In Range");
			}

		}
    }
}
