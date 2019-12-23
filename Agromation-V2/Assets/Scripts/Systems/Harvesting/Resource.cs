using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
	Wood,
	Stone,
	Ore,
	Grass
}
public class Resource : MonoBehaviour
{
	[SerializeField] private ResourceType type;
	[SerializeField] private GameObject resourceToSpawn;
	[SerializeField] private int damage;

	[Tooltip("The amount of the resource to spawn(LOWERBOUND, UPPERBOUND)")]
	[SerializeField] private Vector2 amountToSpawn;

	public ResourceType Type { get => type;}
	

	public void Hit()
	{
		//Take harvesting damage
		damage--;
		//Has the resource been completly destroyed
		if(damage <= 0)
		{
			Destroyed();
		}
	}

	private void Destroyed()
	{
		int spawnAmount = Random.Range((int)amountToSpawn.x, (int)amountToSpawn.y);
		for (int i = 0; i < spawnAmount; i++)
		{
			Instantiate(resourceToSpawn,
				transform.position +(Vector3.up * transform.localScale.y / 2 + Vector3.up * i * resourceToSpawn.transform.localScale.y),
				Quaternion.identity);
		}
		Destroy(this.gameObject);
	}
}
