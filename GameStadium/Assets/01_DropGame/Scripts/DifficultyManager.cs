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
		private SpawnManager _spawnManager;

		private List<CollectorPlayer> _allComPlayers;

		[Header("Easy")]
		public int chanceOfRapidSpawningElementsEasy = 33;
		public int chanceOfBadItemEasy = 33;
		public int chanceOfSimultaneouslySpawnEasy = 25;
		public int COM_ChanceOfDodgeBadItemEasy = 60;
		public int COM_ChanceOfCatchGoodItemEasy = 75;
		public float difficultyMultiplierEasy = 1f;

		[Header("Normal")]
		public int chanceOfRapidSpawningElementsNormal = 44;
		public int chanceOfBadItemNormal = 40;
		public int chanceOfSimultaneouslySpawnMedium = 35;
		public int COM_ChanceOfDodgeBadItemMedium = 70;
		public int COM_ChanceOfCatchGoodItemMedium = 85;
		public float difficultyMultiplierNormal = 1.25f;

		[Header("Hard")]
		public int chanceOfRapidSpawningElementsHard = 66;
		public int chanceOfBadItemHard = 50;
		public int chanceOfSimultaneouslySpawnHard = 45;
		public int COM_ChanceOfDodgeBadItemHard = 80;
		public int COM_ChanceOfCatchGoodItemHard = 98;
		public float difficultyMultiplierHard = 1.5f;

		private void Awake()
		{
			_spawnManager = FindObjectOfType<SpawnManager>();
		}

		public void SetDifficultySettings(Difficulty difficulty)
		{
			float difficultyMultiplier = 0f;
			int chanceOfRapidSpawningElements = 0;
			int chanceOfBadItem = 0;
			int chanceOfSimultaneouslySpawn = 0;
			int COM_ChanceOfDodgeBadItem = 0;
			int COM_ChanceOfCatchGoodItem = 0;

			switch (difficulty)
			{
				case Difficulty.Easy:
					difficultyMultiplier = difficultyMultiplierEasy;
					chanceOfRapidSpawningElements = chanceOfRapidSpawningElementsEasy;
					chanceOfBadItem = chanceOfBadItemEasy;
					chanceOfSimultaneouslySpawn = chanceOfSimultaneouslySpawnEasy;
					COM_ChanceOfDodgeBadItem = COM_ChanceOfDodgeBadItemEasy;
					COM_ChanceOfCatchGoodItem = COM_ChanceOfCatchGoodItemEasy;
					break;
				case Difficulty.Normal:
					difficultyMultiplier = difficultyMultiplierNormal;
					chanceOfRapidSpawningElements = chanceOfRapidSpawningElementsNormal;
					chanceOfBadItem = chanceOfBadItemNormal;
					chanceOfSimultaneouslySpawn = chanceOfSimultaneouslySpawnMedium;
					COM_ChanceOfDodgeBadItem = COM_ChanceOfDodgeBadItemMedium;
					COM_ChanceOfCatchGoodItem = COM_ChanceOfCatchGoodItemMedium;
					break;
				case Difficulty.Hard:
					difficultyMultiplier = difficultyMultiplierHard;
					chanceOfRapidSpawningElements = chanceOfRapidSpawningElementsHard;
					chanceOfBadItem = chanceOfBadItemHard;
					chanceOfSimultaneouslySpawn = chanceOfSimultaneouslySpawnHard;
					COM_ChanceOfDodgeBadItem = COM_ChanceOfDodgeBadItemHard;
					COM_ChanceOfCatchGoodItem = COM_ChanceOfCatchGoodItemHard;
					break;
			}

			_spawnManager.defaultFallSpeed *= difficultyMultiplier;
			_spawnManager.maxFallSpeed *= difficultyMultiplier;
			_spawnManager.defaultSpawnRate -= difficultyMultiplier * (difficultyMultiplier - 1);
			_spawnManager.fastestSpawnRate -= difficultyMultiplier * (difficultyMultiplier - 1);
			_spawnManager.chanceOfRapidSpawningElements = chanceOfRapidSpawningElements;
			_spawnManager.chanceOfBadItem = chanceOfBadItem;
			_spawnManager.chanceOfSimultaneouslySpawn = chanceOfSimultaneouslySpawn;

			foreach (var player in _spawnManager.allComPlayers)
			{
				player.comCatchReactionScript.chanceOfDodgeBadItem = COM_ChanceOfDodgeBadItem;
				player.comCatchReactionScript.chanceOfCatchGoodItem= COM_ChanceOfCatchGoodItem;
			}

			print("Default Fall Speed: " + _spawnManager.defaultFallSpeed);
			print("Max Fall Speed: " + _spawnManager.maxFallSpeed);
			print("Default Spawn Rate: " + _spawnManager.defaultSpawnRate);
			print("Fastest Spawn Rate: " + _spawnManager.fastestSpawnRate);
			print("Chance of Rapid Spawning Elements: " + _spawnManager.chanceOfRapidSpawningElements);
			print("Chance of Bad Items: " + _spawnManager.chanceOfBadItem);
			print("Chance of Simultaneously Spawn: " + _spawnManager.chanceOfSimultaneouslySpawn);
		}
	}
}
