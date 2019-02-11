using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._01_DropGame.Scripts
{
	public class CollectorPlayer : MonoBehaviour
	{
		public bool isComPlayer;

		[HideInInspector]
		public PlayerPosition currenPlayerPosition;

		[Header("Controls")]
		public KeyCode left = KeyCode.A;
		public KeyCode right = KeyCode.D;

		[Header("UI")]
		public Image itemIcon;
		public TextMeshProUGUI scoreText;

		[Header("Spawner")]
		public GameObject spawner;

		[Header("COM - Random Wait Time for Reset")]
		public float waitTimeMin = 0.3f;
		public float waitTimeMax = 0.6f;
		[Header("COM Reaction")]
		public ComCatchReactionScript comCatchReactionScript;

		Vector3 _originalPosition;

		int _score = 0;
		int _goodPoints = 0;
		int _badPoints = 0;
	
		GameManagerDropGame _gameManager;

		private void Awake()
		{
			_gameManager = FindObjectOfType<GameManagerDropGame>();
			
			_originalPosition = transform.position;

			_goodPoints = _gameManager.goodPoints;
			_badPoints = _gameManager.badPoints;

			UpdatePlayerScore(_score);
		}

		public void Update()
		{
			if (!isComPlayer)
			{
				if (Input.GetKeyDown(left))
				{
					if (currenPlayerPosition != PlayerPosition.Left)
					{
						CatchLeft();
					}
				}
				if (Input.GetKeyDown(right))
				{
					if (currenPlayerPosition != PlayerPosition.Right)
					{
						CatchRight();
					}
				}

				if (Input.GetKeyUp(left))
				{
					ResetPosition();
				}
				else if (Input.GetKeyUp(right))
				{
					ResetPosition();
				}
			}
		}

		public void CatchLeft()
		{
			transform.position += new Vector3(-1, 0);
			currenPlayerPosition = PlayerPosition.Left;
		}

		public void CatchRight()
		{
			transform.position += new Vector3(1, 0);
			currenPlayerPosition = PlayerPosition.Right;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "FallingItem")
			{
				ItemScript item = collision.GetComponent<ItemScript>();

				if (item.isBadItem)
				{
					if (_score - _badPoints < 0)
					{
						_score = 0;
					}
					else
					{
						_score -= _badPoints;
					}
				}
				else
				{
					_score += _goodPoints;
				}

				if (isComPlayer)
				{
					ComResetPosition();
				}
				
				UpdatePlayerScore(_score);
				Destroy(collision.gameObject);
			}
		}

		void UpdatePlayerScore(int score)
		{
			scoreText.text = score.ToString();
		}

		public void ComResetPosition()
		{
			StartCoroutine(ComResetPosition_Coroutine());
		}

		public IEnumerator ComResetPosition_Coroutine()
		{
			float randomWaitTime = Random.Range(waitTimeMin, waitTimeMax);

			yield return new WaitForSeconds(randomWaitTime);

			ResetPosition();
		}

		public void ResetPosition()
		{
			transform.position = _originalPosition;
			currenPlayerPosition = PlayerPosition.Center;
		}
	}

	public enum PlayerPosition
	{
		Left,
		Center,
		Right
	}
}
