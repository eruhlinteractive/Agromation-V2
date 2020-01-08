using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
	[SerializeField] private AudioSource audSrc;
	[SerializeField] private AudioClip phoneStatic;
	[SerializeField] private AudioClip introClipOne;
	[SerializeField] private AudioClip introClipTwo;
	[SerializeField] private Vector3 chestSpawnPos;
	public GameObject dropChestPrefab;
	[SerializeField] private List<int> startingChestItems;

	DropChest startingChest;
	private void Start()
	{
		StartCoroutine(PlayIntroSequence());
	}



	/// <summary>
	/// The intro voice line sequence 
	/// </summary>
	/// <returns></returns>
	IEnumerator PlayIntroSequence()
	{
		//First Part
		yield return new WaitForSeconds(1f);
		audSrc.PlayOneShot(phoneStatic);
		yield return new WaitForSeconds(1f);
		audSrc.PlayOneShot(introClipOne);

		//Wait until clip is done to continue
		while (audSrc.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(1f);
		audSrc.PlayOneShot(phoneStatic);
		SpawnDropCrate();
		yield return new WaitForSeconds(1f);
		


		//Second Part
		while (!startingChest.HasBeenOpened)
		{
			yield return new WaitForSeconds(0.25f);
		}
		yield return new WaitForSeconds(2f);
		audSrc.PlayOneShot(phoneStatic);
		yield return new WaitForSeconds(1f);
		audSrc.PlayOneShot(introClipTwo);

		//Wait until clip is done to continue
		while (audSrc.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(1f);
		audSrc.PlayOneShot(phoneStatic);
		this.enabled = false;
	}



	private void SpawnDropCrate()
	{
		startingChest = Instantiate(dropChestPrefab, chestSpawnPos,Quaternion.identity).GetComponent<DropChest>();
		startingChest.CreateChestItems(startingChestItems);
	}
}
