using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	float size = 1;


	private static Grid instance;
	public static Grid Instance { get { return instance; } }


	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {

		//PlaceGround();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public Vector3 GetNearestPointOnGrid(Vector3 position)
	{
		position -= transform.position;

		int xCount = Mathf.RoundToInt(position.x / size);
		int yCount = Mathf.RoundToInt(position.y / size);
		int zCount = Mathf.RoundToInt(position.z / size);

		Vector3 result = new Vector3
			(
			(float)xCount * size,
			(float)yCount * size,
			(float)zCount * size
			);

		result += transform.position;
		return result;

	}

	public Vector3 GetNearestPointOnGridWithY(Vector3 position)
	{
		position -= transform.position;

		int xCount = Mathf.RoundToInt(position.x / size);
		int yCount = Mathf.RoundToInt(position.y / size);
		int zCount = Mathf.RoundToInt(position.z / size);

		Vector3 result = new Vector3
			(
			(float)xCount * size,
			position.y,
			(float)zCount * size
			);

		result += transform.position;
		return result;

	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		for (float x = -5; x < 11; x += size)
		{
			for (float z = -5; z < 11; z += size)
			{
				Vector3 point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
				Gizmos.DrawSphere(point, 0.1f);
			}

		}
	}

//private void PlaceGround()
//{
//	for (float x = -5; x < 11; x++)
//
//	{
//		for (float z = -5; z < 11; z += size)
//		{
//			Vector3 point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
//			Instantiate(ground, point , Quaternion.Euler(-90, 0, 0));
//		}
//	}
//}
}
