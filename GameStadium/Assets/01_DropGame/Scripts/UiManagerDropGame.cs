using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManagerDropGame : MonoBehaviour
{
	 TextMeshProUGUI dropCountText;

	 private void Awake()
	 {
		  dropCountText = GameObject.FindGameObjectWithTag("GamePoints").GetComponent<TextMeshProUGUI>();
	 }

	 public void UpdateDropCount(int count)
	 {
		  dropCountText.text = count.ToString();
	 }
}
