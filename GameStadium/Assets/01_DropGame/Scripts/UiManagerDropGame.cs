using TMPro;
using UnityEngine;

namespace Assets._01_DropGame.Scripts
{
	public class UiManagerDropGame : MonoBehaviour
	{
		TextMeshProUGUI _dropCountText;

		private void Awake()
		{
			_dropCountText = GameObject.FindGameObjectWithTag("GamePoints").GetComponent<TextMeshProUGUI>();
		}

		public void UpdateDropCount(int count)
		{
			_dropCountText.text = count.ToString();
		}
	}
}
