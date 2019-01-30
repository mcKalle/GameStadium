using UnityEngine;

namespace Assets._01_DropGame.Scripts
{
	public class GameManagerDropGame : MonoBehaviour
	{

		public int countOfDropObjects = 20;
		public int goodPoints = 1;
		public int badPoints = 3;

		public Difficulty gameDifficulty;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		[HideInInspector]
		public enum Difficulty
		{
			Easy,
			Normal,
			Hard
		}
	}
}
