using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warper : MonoBehaviour
{
	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.layer == 12) { // 12 - Warp
			Vector3 position = transform.position;
			position.x = position.x * -1;
			transform.position = position;
		}
	}
}
