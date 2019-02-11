using UnityEngine;

namespace Assets._01_DropGame.Scripts
{
	public class ItemScript : MonoBehaviour
	{
		public new Rigidbody2D rigidbody2D;

		public bool isBadItem;

		public bool spawnedWithSecondItem = false;

		public ItemPosition position;

		public GameObject spawner;

		private void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
		}

	}

	public enum ItemPosition
	{
		Left,
		Center,
		Right
	}
}
