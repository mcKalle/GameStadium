using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

using Difficulty = Assets._01_DropGame.Scripts.GameManagerDropGame.Difficulty;

namespace Assets._01_DropGame.Scripts
{
	public class DifficultyManager : MonoBehaviour
	{
		private GameManagerDropGame _gameManager;
		private SpawnManager _spawnManager;

		[Header("Easy")]
		public int chanceOfRapidSpawningElementsEasy = 33;
		public int chanceOfBadItemEasy = 33;
		public float difficultyMultiplierEasy = 1f;

		[Header("Normal")]
		public int chanceOfRapidSpawningElementsNormal = 44;
		public int chanceOfBadItemNormal = 40;
		public float difficultyMultiplierNormal = 1.25f;

		[Header("Hard")]
		public int chanceOfRapidSpawningElementsHard = 66;
		public int chanceOfBadItemHard = 50;
		public float difficultyMultiplierHard = 1.5f;

		private void Awake()
		{
			_gameManager = FindObjectOfType<GameManagerDropGame>();
			_spawnManager = FindObjectOfType<SpawnManager>();
		}

		public void SetDifficultySettings(Difficulty difficulty)
		{
			float difficultyMultiplier = 0f;
			int chanceOfRapidSpawningElements = 0;
			int chanceOfBadItem = 0;

			switch (difficulty)
			{
				case Difficulty.Easy:
					difficultyMultiplier = difficultyMultiplierEasy;
					chanceOfRapidSpawningElements = chanceOfRapidSpawningElementsEasy;
					chanceOfBadItem = chanceOfBadItemEasy;
					break;
				case Difficulty.Normal:
					difficultyMultiplier = difficultyMultiplierNormal;
					chanceOfRapidSpawningElements = chanceOfRapidSpawningElementsNormal;
					chanceOfBadItem = chanceOfBadItemNormal;
					break;
				case Difficulty.Hard:
					difficultyMultiplier = difficultyMultiplierHard;
					chanceOfRapidSpawningElements = chanceOfRapidSpawningElementsHard;
					chanceOfBadItem = chanceOfBadItemHard;
					break;
			}

			_spawnManager.defaultFallSpeed *= difficultyMultiplier;
			_spawnManager.maxFallSpeed *= difficultyMultiplier;
			_spawnManager.defaultSpawnRate -= difficultyMultiplier * (difficultyMultiplier - 1);
			_spawnManager.fastestSpawnRate -= difficultyMultiplier * (difficultyMultiplier - 1);
			_spawnManager.chanceOfRapidSpawningElements = chanceOfRapidSpawningElements;
			_spawnManager.chanceOfBadItem = chanceOfBadItem;

			print("Default Fall Speed: " + _spawnManager.defaultFallSpeed);
			print("Max Fall Speed: " + _spawnManager.maxFallSpeed);
			print("Default Spawn Rate: " + _spawnManager.defaultSpawnRate);
			print("Fastest Spawn Rate: " + _spawnManager.fastestSpawnRate);
			print("Chance of Rapid Spawning Elements: " + _spawnManager.chanceOfRapidSpawningElements);
			print("Chance of Bad Items: " + _spawnManager.chanceOfBadItem);
		}
	}
}
