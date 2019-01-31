using System;
using TMPro;
using UnityEngine;

using Difficulty = Assets._01_DropGame.Scripts.GameManagerDropGame.Difficulty;

namespace Assets._01_DropGame.Scripts
{
	public class UiManagerDropGame : MonoBehaviour
	{
		public TextMeshProUGUI difficultyLabel;

		TextMeshProUGUI _dropCountText;

		private void Awake()
		{
			_dropCountText = GameObject.FindGameObjectWithTag("GamePoints").GetComponent<TextMeshProUGUI>();
		}

		public void UpdateDropCount(int count)
		{
			_dropCountText.text = count.ToString();
		}

		public void PrintDifficulty(Difficulty gameDifficulty)
		{
			difficultyLabel.text = Enum.GetName(typeof(Difficulty), gameDifficulty);
		}
	}
}
