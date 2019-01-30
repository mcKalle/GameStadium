using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectorPlayer : MonoBehaviour
{

	 [Header("Controls")]
	 public KeyCode Left = KeyCode.A;
	 public KeyCode Right = KeyCode.D;

	 [Header("UI")]
	 public Image itemIcon;
	 public TextMeshProUGUI scoreText;

	 Vector3 originalPosition;

	 int score = 0;
	 int goodPoints = 0;
	 int badPoints = 0;

	 GameManagerDropGame gameManager;

	 private void Start()
	 {
		  gameManager = FindObjectOfType<GameManagerDropGame>();

		  originalPosition = transform.position;

		  goodPoints = gameManager.GoodPoints;
		  badPoints = gameManager.BadPoints;

		  UpdatePlayerScore(score);
	 }

	 public void Update()
	 {
		  if (Input.GetKeyDown(Left))
		  {
				CatchLeft();
		  }
		  if (Input.GetKeyDown(Right))
		  {
				CatchRight();
		  }

		  if (Input.GetKeyUp(Left))
		  {
				CatchRight();
		  }
		  else if (Input.GetKeyUp(Right))
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
				
				if (score - badPoints < 0)
				{
					 score = 0;
				}
				else
				{
					 score -= badPoints;
				}

				UpdatePlayerScore(score);
				Destroy(collision.gameObject);
		  }
		  else if (collision.tag == "GoodItem")
		  {
				score += goodPoints;

				UpdatePlayerScore(score);
				Destroy(collision.gameObject);
		  }
	 }

	 void UpdatePlayerScore(int score)
	 {
		  scoreText.text = score.ToString();
	 }
}
