using UnityEngine;
using System.Collections;

public class CoolCamera : MonoBehaviour
{
	[SerializeField]
	private Vector3 offset;

	[SerializeField]
	private float time = 0.1f;

	[SerializeField]
	private bool isHorizontalLocked;

	[SerializeField]
	private Transform target;

	void Start ()
	{
		if (target == null) {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			target = player.transform;
		}
	}

	void Update ()
	{
		if (target != null) {
			Vector3 goalPosition = target.position;
			goalPosition.z = transform.position.z;

			if (isHorizontalLocked) {
				goalPosition.x = transform.position.x;
			}

			transform.position = Vector3.Lerp (transform.position, goalPosition + offset, time);
		}
	}
}