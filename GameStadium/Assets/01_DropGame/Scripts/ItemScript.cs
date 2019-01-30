using UnityEngine;

namespace Assets._01_DropGame.Scripts
{
	public class ItemScript : MonoBehaviour
	{

		public new Rigidbody2D rigidbody2D;

		private void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
		}

	}
}
