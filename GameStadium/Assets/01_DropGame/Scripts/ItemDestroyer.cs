using UnityEngine;

namespace Assets._01_DropGame.Scripts
{
	public class ItemDestroyer : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "GoodItem" || collision.tag == "BadItem")
			{
				Destroy(collision.gameObject);
			}
		}

	}
}
