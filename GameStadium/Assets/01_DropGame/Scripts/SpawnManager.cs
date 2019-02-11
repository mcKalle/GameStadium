using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

namespace Assets._01_DropGame.Scripts
{
	public class SpawnManager : MonoBehaviour
	{
		public GameObject goodItem;
		public GameObject badItem;
		public int chanceOfBadItem = 20;

		// spawning rate
		// this is the normal spawn rate
		public float defaultSpawnRate = 1.4f;
		// this is the spawn rate for elements, which spawn shortly after each other
		public float fastestSpawnRate = 0.8f;

		public int chanceOfRapidSpawningElements;
		public int chanceOfSimultaneouslySpawn;

		// flag if rapid spawning is enabled
		private bool _spawnMultipleElementsRapidly;

		// sometimes elements spawn very fast, almost at once
		// this value represents the maximum count
		public int maxCountOfElementsSpawnedFaster = 3;
		// actual (random) number of rapid spawning elements
		private int _countOfFastSpawningElements;

		public float defaultFallSpeed = 1f;
		public float maxFallSpeed = 1.5f;

		//public float decreaseFactorSpawningRate = 0.1f;
		public float increaseFactorFallSpeed = 0.2f;

		public int changeFactorEveryXDropElement = 30;

		private int _countOfDropObjects;

		private float _spawningRate;
		private float _fallSpeed;

		private GameManagerDropGame _gameManager;
		private DifficultyManager _difficultyManager;
		private UiManagerDropGame _uiManager;

		private List<GameObject> _playerSpawners;

		[HideInInspector]
		public List<CollectorPlayer> allComPlayers;

		private void Start()
		{
			_gameManager = FindObjectOfType<GameManagerDropGame>();
			_difficultyManager = FindObjectOfType<DifficultyManager>();
			_uiManager = FindObjectOfType<UiManagerDropGame>();

			_countOfDropObjects = _gameManager.countOfDropObjects;
			_uiManager.UpdateDropCount(_countOfDropObjects);

			_spawningRate = defaultSpawnRate;
			_fallSpeed = defaultFallSpeed;

			// get all player spawner
			_playerSpawners = GameObject.FindGameObjectsWithTag("PlayerSpawner").ToList();

			allComPlayers = FindObjectsOfType<CollectorPlayer>().ToList().Where(player => player.isComPlayer).ToList();

			_difficultyManager.SetDifficultySettings(_gameManager.gameDifficulty);
			_uiManager.PrintDifficulty(_gameManager.gameDifficulty);

			StartCoroutine(Spawing_Coroutine());
		}

		public IEnumerator Spawing_Coroutine()
		{
			// set this variables, so the first elements are always single elements with default speed
			_countOfFastSpawningElements = 10;
			_spawnMultipleElementsRapidly = true;

			while (_countOfDropObjects > 0)
			{
				// spawn & only decrease if a good item was spawned
				bool decreaseCounter = SpawnObject();
				if (!decreaseCounter)
				{
					_countOfDropObjects--;
				}

				if (_countOfDropObjects % changeFactorEveryXDropElement == 0)
				{
					ChangeFactorValues();
				}

				_uiManager.UpdateDropCount(_countOfDropObjects);

				// calculate if the next spawns are faster
				if (_countOfFastSpawningElements == 0)
				{
					_spawnMultipleElementsRapidly = RandomChance.GetRandomChance(chanceOfRapidSpawningElements);

					if (_spawnMultipleElementsRapidly)
					{
						// get how much shall spawn rapidly
						_countOfFastSpawningElements = Random.Range(1, maxCountOfElementsSpawnedFaster + 1);
						// calculate random rate for faster spawning
						_spawningRate = Random.Range(defaultSpawnRate, fastestSpawnRate);
						// calculate random speed for faster spawning
						_fallSpeed = Random.Range(defaultFallSpeed, maxFallSpeed);
					}
				}

				yield return new WaitForSeconds(_spawningRate);

				// decrease counter of rapid spawning elements
				if (_spawnMultipleElementsRapidly)
				{
					_countOfFastSpawningElements--;
				}

				// reset rate and speed
				if (_countOfFastSpawningElements == 0)
				{
					_spawningRate = defaultSpawnRate;
					_fallSpeed = defaultFallSpeed;
				}
			}
		}

		bool SpawnObject()
		{
			//do it for all found spawners

			// add item to COM queue only one time
			bool alreadyAddedToComQueue = false;

			// calculate the global random values
			int randomPosition = Random.Range(-1, 2);

			// a random item is also spawned with a 20% chance
			bool spawnBadItem = RandomChance.GetRandomChance(chanceOfBadItem);

			// a good and bad item spawn simultaneously with a 50% chance 
			bool spawnSimultaneously = false;
			int simultaneouslyRandomPosition = 0;

			if (spawnBadItem)
			{
				spawnSimultaneously = RandomChance.GetRandomChance(chanceOfSimultaneouslySpawn);

				// get a value for simultaneously spawn, which is not equal to the main spawn position
				do
				{
					simultaneouslyRandomPosition = Random.Range(-1, 2);
				} while (simultaneouslyRandomPosition == randomPosition);
			}

			foreach (GameObject spawner in _playerSpawners)
			{
				GameObject spawnItem;

				Vector3 newPos = new Vector3(spawner.transform.position.x, spawner.transform.position.y);

				// if chance was hit, bad item is spawned
				if (spawnBadItem)
				{
					if (spawnSimultaneously)
					{
						// also spawn good item, but at a different location
						GameObject simultaneousSpawnItemObject = Instantiate(goodItem, newPos, Quaternion.identity);

						simultaneousSpawnItemObject.transform.position = new Vector3(spawner.transform.position.x + simultaneouslyRandomPosition,
							spawner.transform.position.y);

						ItemScript simultaneousSpawnItem = simultaneousSpawnItemObject.GetComponent<ItemScript>();
						simultaneousSpawnItem.isBadItem = false;
						simultaneousSpawnItem.spawner = spawner;
						// save the position in the object for later usage, because once set, it becomes a global value
						SavePositionForItem(simultaneousSpawnItem, simultaneouslyRandomPosition);

						simultaneousSpawnItemObject.GetComponent<Rigidbody2D>().gravityScale = _fallSpeed;
					}

					spawnItem = Instantiate(badItem, newPos, Quaternion.identity);
				}
				else
				{
					spawnItem = Instantiate(goodItem, newPos, Quaternion.identity);
				}
				ItemScript item = spawnItem.GetComponent<ItemScript>();
				item.isBadItem = spawnBadItem;
				item.spawner = spawner;
				// save if a "simultaneous" spawn was performed
				item.spawnedWithSecondItem = spawnSimultaneously;
				// save the position in the object for later usage, because once set, it becomes a global value
				SavePositionForItem(item, randomPosition);

				spawnItem.GetComponent<Rigidbody2D>().gravityScale = _fallSpeed;

				spawnItem.transform.position = new Vector3(spawner.transform.position.x + randomPosition,
					spawner.transform.position.y);

				// put the item to the COM reaction queue
				// only do this one time (we are in a loop for each spawner, this info is only need once for the ComCatchReactionScript)
				if (!alreadyAddedToComQueue)
				{
					allComPlayers.ForEach(comPlayer => comPlayer.comCatchReactionScript.AddItemToReactionList(spawnItem));

					alreadyAddedToComQueue = true;
				}
			}

			// reset this so the counter is decreased
			if (spawnSimultaneously)
			{
				spawnBadItem = false;
			}

			return spawnBadItem;
		}

		void SavePositionForItem(ItemScript item, int randomXPosition)
		{
			switch (randomXPosition)
			{
				case -1:
					item.position = ItemPosition.Left;
					break;
				case 0:
					item.position = ItemPosition.Center;
					break;
				case 1:
					item.position = ItemPosition.Right;
					break;
			}
		}

		void ChangeFactorValues()
		{
			Debug.Log(string.Format("Count is {0}, old spawning rate is {1}, old fallSpeed is {2}",
				_countOfDropObjects, _spawningRate, _fallSpeed));

			//_spawningRate -= decreaseFactorSpawningRate;
			defaultFallSpeed += increaseFactorFallSpeed;
			maxFallSpeed += increaseFactorFallSpeed;

			Debug.Log(string.Format("New spawning rate is {0}, new fallSpeed is {1}",
				_spawningRate, _fallSpeed));
		}
	}
}
