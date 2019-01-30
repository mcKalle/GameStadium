using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public GameObject goodItem;
	public GameObject badItem;
	public int ChanceForBadItemInPercentage = 20;

	public float InitalSpawningRate = 1.2f;
	public float InitalFallSpeed = 1f;

	public float decreaseFactorSpawningRate = 0.1f;
	public float increaseFactorFallSpeed = 0.2f;

	public int changeFactorEveryXDropElement = 10;

	int countOfDropObjects;

	float spawningRate;
	float fallSpeed;

	GameManagerDropGame gameManager;
	UiManagerDropGame uiManager;

	List<GameObject> playerSpawners;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManagerDropGame>();
		uiManager = FindObjectOfType<UiManagerDropGame>();

		countOfDropObjects = gameManager.CountOfDropObjects;
		uiManager.UpdateDropCount(countOfDropObjects);

		spawningRate = InitalSpawningRate;
		fallSpeed = InitalFallSpeed;

		// get all player spawner
		playerSpawners = GameObject.FindGameObjectsWithTag("PlayerSpawner").ToList();
	}

	private void Start()
	{
		StartCoroutine(Spawing_Coroutine());
	}

	public IEnumerator Spawing_Coroutine()
	{
		while (countOfDropObjects > 0)
		{

			// spawn & only decrease if a good item was spawned
			bool decreaseCounter = SpawnObject();
			if (!decreaseCounter)
			{
				countOfDropObjects--;
			}

			if (countOfDropObjects % changeFactorEveryXDropElement == 0)
			{
				Debug.Log(string.Format("Count is {0}, old spawning rate is {1}, old fallSpeed is {2}",
					 countOfDropObjects, spawningRate, fallSpeed));

				spawningRate -= decreaseFactorSpawningRate;
				fallSpeed += increaseFactorFallSpeed;

				Debug.Log(string.Format("New spawning rate is {1}, new fallSpeed is {2}",
					countOfDropObjects, spawningRate, fallSpeed));
			}

			uiManager.UpdateDropCount(countOfDropObjects);

			yield return new WaitForSeconds(spawningRate);
		}
	}

	bool SpawnObject()
	{
		//do it for all found spawners

		// calculate the global random values
		int randomPosition = Random.Range(-1, 2);

		// a random item is also spawned with a 20% chance
		bool spawnBadItem = RandomChance.GetRandomChance(20);

		// a good and bad item spawn simultaneously with a 50% chance 
		bool spawnSimultaneously = false;
		int simulatneouslyRandomPosition = 0;

		if (spawnBadItem)
		{
			spawnSimultaneously = RandomChance.GetRandomChance(50);

			// get a value for simultaneously spawn, which is not equal to the main spawn position
			do
			{
				simulatneouslyRandomPosition = Random.Range(-1, 2);
			} while (simulatneouslyRandomPosition == randomPosition);
		}

		foreach (GameObject spawner in playerSpawners)
		{
			GameObject spawnItem;

			Vector3 newPos = new Vector3(spawner.transform.position.x, spawner.transform.position.y);

			// if chance was hit, bad item is spawned
			if (spawnBadItem)
			{
				if (spawnSimultaneously)
				{
					// also spawn good item, but at a different location
					GameObject simultaneousSpawnItem = Instantiate(goodItem, newPos, Quaternion.identity);
					simultaneousSpawnItem.GetComponent<Rigidbody2D>().gravityScale = fallSpeed;

					simultaneousSpawnItem.transform.position = new Vector3(spawner.transform.position.x + simulatneouslyRandomPosition,
						spawner.transform.position.y);

					
				}

				spawnItem = Instantiate(badItem, newPos, Quaternion.identity);
			}
			else
			{
				spawnItem = Instantiate(goodItem, newPos, Quaternion.identity);
			}

			spawnItem.GetComponent<Rigidbody2D>().gravityScale = fallSpeed;

			spawnItem.transform.position = new Vector3(spawner.transform.position.x + randomPosition,
				 spawner.transform.position.y);
		}


		// reset this so the counter is decreased
		if (spawnSimultaneously)
		{
			spawnBadItem = false;
		}

		return spawnBadItem;
	}
}
