using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropChest : MonoBehaviour
{
	[SerializeField] private GameObject parachute;
	[SerializeField] bool isGrounded = false;
	[SerializeField] float moveSpeed;
	RaycastHit groundHit;



	//Loot spawn variables
	[SerializeField] List<int> itemsToSpawn = new List<int>();
	private ItemManager _itemManager;
	[SerializeField] private Transform itemSpawnPoint;
	[SerializeField] Animator animator;

	private bool hasBeenOpened = false;


	private void Start()
	{
		_itemManager = GameSettings.Instance.ItemManager;
	}

	private void Update()
	{
		//Cast ray
		Physics.Raycast(transform.position, Vector3.down, out groundHit, Mathf.Infinity);
		Debug.DrawRay(transform.position, Vector3.down * Mathf.Infinity);
	}
	// Update is called once per frame
	void FixedUpdate()
    {
		if (!isGrounded)
		{
			if (Vector3.Distance(transform.position, groundHit.point) > 5f)
			{
				Debug.Log("NearGround");
				transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down, moveSpeed * Time.fixedDeltaTime);
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down, moveSpeed /2 * Time.fixedDeltaTime);
			}
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Ground"))
		{ 
			isGrounded = true;
			parachute.SetActive(false);

			//Make the rigidbody kinematic
			this.GetComponent<Rigidbody>().isKinematic = true;
			//Set layer
			this.gameObject.layer = 0;
		}

	}

	public void OpenChest()
	{
		//Start coroutine IF it hasnt been opened yet
		if (!hasBeenOpened)
		{
			StartCoroutine(SpawnItems());
		}
		hasBeenOpened = true;

	}


	IEnumerator SpawnItems()
	{
		//Open the chest
		animator.SetTrigger("Open");
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < itemsToSpawn.Count; i++)
		{

			GameObject item = Instantiate(_itemManager.GetItem(itemsToSpawn[i]), itemSpawnPoint.transform.position, Quaternion.identity);
			item.AddComponent<DropChestItem>();
			yield return new WaitForSeconds(0.2f);
			
		}

		Destroy(this.gameObject);
	}
}
