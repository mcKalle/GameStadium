using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._01_DropGame.Scripts
{
	public class CollectorPlayer : MonoBehaviour
	{

		[Header("Controls")]
		public KeyCode left = KeyCode.A;
		public KeyCode right = KeyCode.D;

		[Header("UI")]
		public Image itemIcon;
		public TextMeshProUGUI scoreText;

		Vector3 _originalPosition;

		int _score = 0;
		int _goodPoints = 0;
		int _badPoints = 0;

		GameManagerDropGame _gameManager;

		private void Start()
		{
			_gameManager = FindObjectOfType<GameManagerDropGame>();

			_originalPosition = transform.position;

			_goodPoints = _gameManager.goodPoints;
			_badPoints = _gameManager.badPoints;

			UpdatePlayerScore(_score);
		}

		public void Update()
		{
			if (Input.GetKeyDown(left))
			{
				CatchLeft();
			}
			if (Input.GetKeyDown(right))
			{
				CatchRight();
			}

			if (Input.GetKeyUp(left))
			{
				CatchRight();
			}
			else if (Input.GetKeyUp(right))
			{
				CatchLeft();
			}
		}

		void CatchLeft()
		{
			transform.position += new Vector3(-1, 0);
		}

		void CatchRight()
		{
			transform.position += new Vector3(1, 0);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "BadItem")
			{
				
				if (_score - _badPoints < 0)
				{
					_score = 0;
				}
				else
				{
					_score -= _badPoints;
				}

				UpdatePlayerScore(_score);
				Destroy(collision.gameObject);
			}
			else if (collision.tag == "GoodItem")
			{
				_score += _goodPoints;

				UpdatePlayerScore(_score);
				Destroy(collision.gameObject);
			}
		}

		void UpdatePlayerScore(int score)
		{
			scoreText.text = score.ToString();
		}
	}
}
