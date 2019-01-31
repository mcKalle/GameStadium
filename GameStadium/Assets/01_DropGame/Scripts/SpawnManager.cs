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

		private void Awake()
		{
			_gameManager = FindObjectOfType<GameManagerDropGame>();
			_difficultyManager = FindObjectOfType<DifficultyManager>();
			_uiManager = FindObjectOfType<UiManagerDropGame>();

			_countOfDropObjects = _gameManager.countOfDropObjects;
			_uiManager.UpdateDropCount(_countOfDropObjects);

			_difficultyManager.SetDifficultySettings(_gameManager.gameDifficulty);
			_uiManager.PrintDifficulty(_gameManager.gameDifficulty);

			_spawningRate = defaultSpawnRate;
			_fallSpeed = defaultFallSpeed;

			// get all player spawner
			_playerSpawners = GameObject.FindGameObjectsWithTag("PlayerSpawner").ToList();
		}

		private void Start()
		{
			StartCoroutine(Spawing_Coroutine());
		}

		public IEnumerator Spawing_Coroutine()
		{
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

				if (_spawnMultipleElementsRapidly)
				{
					_countOfFastSpawningElements--;
				}

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

			// calculate the global random values
			int randomPosition = Random.Range(-1, 2);

			// a random item is also spawned with a 20% chance
			bool spawnBadItem = RandomChance.GetRandomChance(20);

			// a good and bad item spawn simultaneously with a 50% chance 
			bool spawnSimultaneously = false;
			int simultaneouslyRandomPosition = 0;

			if (spawnBadItem)
			{
				spawnSimultaneously = RandomChance.GetRandomChance(50);

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
						GameObject simultaneousSpawnItem = Instantiate(goodItem, newPos, Quaternion.identity);
						simultaneousSpawnItem.GetComponent<Rigidbody2D>().gravityScale = _fallSpeed;

						simultaneousSpawnItem.transform.position = new Vector3(spawner.transform.position.x + simultaneouslyRandomPosition,
							spawner.transform.position.y);


					}

					spawnItem = Instantiate(badItem, newPos, Quaternion.identity);
				}
				else
				{
					spawnItem = Instantiate(goodItem, newPos, Quaternion.identity);
				}

				spawnItem.GetComponent<Rigidbody2D>().gravityScale = _fallSpeed;

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
