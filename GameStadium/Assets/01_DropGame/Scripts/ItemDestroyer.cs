using UnityEngine;

namespace Assets._01_DropGame.Scripts
{
	public class ItemDestroyer : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "FallingItem")
			{
				// get player by the spawner object
				GameObject playerArea = collision.gameObject.GetComponent<ItemScript>().spawner.transform.parent.gameObject;
				CollectorPlayer player = playerArea.GetComponentInChildren<CollectorPlayer>();

				if (player.isComPlayer)
				{
					player.ResetPosition();

				}
				
				Destroy(collision.gameObject);
			}
		}

	}
}
