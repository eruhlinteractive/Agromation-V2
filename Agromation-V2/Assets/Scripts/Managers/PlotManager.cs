using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
	Dictionary<Vector3, GameObject> plotIndex = new Dictionary<Vector3, GameObject>();
	Dictionary<Vector3, GameObject> plantingPlots = new Dictionary<Vector3, GameObject>();
	Dictionary<Vector3, GameObject> fences = new Dictionary<Vector3, GameObject>();

	//TESTING
	public GameObject sphere;

	private Grid _grid;
	private static PlotManager instance;
	public static PlotManager Instance { get { return instance; } }

	public Dictionary<Vector3, GameObject> PlantingPlots { get => plantingPlots; }

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
		_grid = Grid.Instance;
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
		//Add the current plot
		plotIndex.Add(pos, newPlot);

		if (newPlot.GetComponent<Fence>() != null)
		{
			fences.Add(pos,newPlot);

			//Get a list of all adjacent posts
			List<Vector3> adjacentPosts = CheckForAdjacentFencePosts(pos);
			//Debug.Log(adjacentPosts.Count);
			//If there is an adjacent plot
			if (adjacentPosts.Count != 0)
			{
				//For every adjacent plot
				for (int i = 0; i < adjacentPosts.Count; i++)
				{
					
					Vector3 posToPlace = (pos - adjacentPosts[i]) / 2 + adjacentPosts[i];
					posToPlace = new Vector3(posToPlace.x, 1, posToPlace.z);
					Instantiate(sphere, posToPlace, Quaternion.LookRotation(pos - adjacentPosts[i]));
				}
			
			}
		}
		else if(newPlot.GetComponent<Plot>() != null)
		{
			//Debug.Log("added plantable plot");
			plantingPlots.Add(pos,newPlot);
		}
	}
	
	/// <summary>
	/// Destroys the plot and removes it from the plotIndex
	/// </summary>
	/// <param name="pos">The position of the plot</param>
	public void RemovePlot(Vector3 pos)
	{
		//Destroy the plot
		Destroy(plotIndex[pos]);
		
		//Remove planting plot
		if (plantingPlots.ContainsKey(pos))
		{
			plantingPlots.Remove(pos);
			Debug.Log("Remove plantable plot");
		}
		//Remove it from index
		plotIndex.Remove(pos);

		//TODO: Add fence removal code
	}

	/// <summary>
	/// Check if the plot is in the plotIndex
	/// </summary>
	/// <param name="pos">The position to check</param>
	/// <returns>True if the plot is in the plotIndex</returns>
	private bool HasPlot(Vector3 pos)
	{
		if (plotIndex.ContainsKey(pos))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Get the plot index of the plot at a given position
	/// </summary>
	/// <param name="pos">The position of the plot to get</param>
	/// <returns>The plot gameobject</returns>
	public GameObject GetPlot(Vector3 pos)
	{
		if (HasPlot(pos))
		{
			return plotIndex[pos];
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// Checks each of the cardinal directions around the post to see if there is another fence post
	/// </summary>
	/// <param name="pos">The position of the newly placed fence post</param>
	/// <returns>A list of all adjacent posts</returns>
	public List<Vector3> CheckForAdjacentFencePosts(Vector3 pos)
	{
		List<Vector3> adjacentPosts = new List<Vector3>();

		if (fences.ContainsKey(_grid.GetNearestPointOnGridWithY(pos + Vector3.forward * _grid.Size)))
		{
			adjacentPosts.Add(_grid.GetNearestPointOnGridWithY(pos + Vector3.forward* _grid.Size));
		}

		if (fences.ContainsKey(_grid.GetNearestPointOnGridWithY(pos + Vector3.back * _grid.Size)))
		{
			adjacentPosts.Add(_grid.GetNearestPointOnGridWithY(pos + Vector3.back * _grid.Size));
		}

		if (fences.ContainsKey(_grid.GetNearestPointOnGridWithY(pos + Vector3.right * _grid.Size)))
		{
			adjacentPosts.Add(_grid.GetNearestPointOnGridWithY(pos + Vector3.right * _grid.Size));
		}

		 if (fences.ContainsKey(_grid.GetNearestPointOnGridWithY(pos + Vector3.left * _grid.Size)))
		{
			adjacentPosts.Add(_grid.GetNearestPointOnGridWithY(pos + Vector3.left * _grid.Size));
		}

		return adjacentPosts;
	}
}
