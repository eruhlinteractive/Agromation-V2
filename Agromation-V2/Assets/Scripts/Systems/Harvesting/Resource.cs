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
	[SerializeField] private int amountToSpawn;

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
		
		for (int i = 0; i < amountToSpawn; i++)
		{
			Instantiate(resourceToSpawn,
				transform.position +(Vector3.up * transform.localScale.y / 2 + Vector3.up * i * resourceToSpawn.transform.localScale.y),
				Quaternion.identity);
		}
		Destroy(this.gameObject);
	}
}
