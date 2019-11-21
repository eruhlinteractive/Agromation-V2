using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
	Dictionary<Vector3, GameObject> plotIndex = new Dictionary<Vector3, GameObject>();
	private static PlotManager instance;
	public static PlotManager Instance { get { return instance; } }

	private void Awake()
	{
		//Set singleton
		if (instance == null)
		{
			instance = this;
		}
	}
	// Start is called before the first frame update
	void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	/// <summary>
	/// Check if a new plot can be placed here 
	/// </summary>
	/// <param name="positionToCheck">The position on the grid to check for</param>
	/// <returns>True if the spot is empty</returns>
	public bool OpenSpot(Vector3 positionToCheck)
	{
		if (plotIndex.ContainsKey(positionToCheck))
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	/// <summary>
	/// Adds plot to list of all plots
	/// </summary>
	/// <param name="pos">The position of the new plot</param>
	/// <param name="newPlot">The new plot gameObject</param>
	public void AddPlot(Vector3 pos, GameObject newPlot)
	{
		plotIndex.Add(pos, newPlot);
	}
}
