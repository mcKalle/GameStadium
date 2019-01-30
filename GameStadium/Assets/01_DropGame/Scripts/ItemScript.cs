using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

	 public new Rigidbody2D rigidbody2D;

	 private void Awake()
	 {
		  rigidbody2D = GetComponent<Rigidbody2D>();
	 }

}
