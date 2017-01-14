using UnityEngine;
using System.Collections;

public class CoolCamera : MonoBehaviour
{
	[SerializeField]
	private Vector3 offset;

	[SerializeField]
	private float time = 0.5f;

	[SerializeField]
	private bool isHorizontalLocked;

	[SerializeField]
	private Transform target;

	private Transform myTransform;

	private float interpVelocity;

	private Vector3 targetPos;

	void Awake ()
	{
		myTransform = GetComponent<Transform> ();
	}

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
			Vector3 originalPosition = myTransform.position;
			Vector3 goalPosition = myTransform.position;

			interpVelocity = 

			myTransform.position = Vector3.Lerp (myTransform.position, goalPosition + offset, time);
		}
		
//		if (target == null) {
//			Vector3 posNoZ = myTransform.position;
//			posNoZ.z = target.position.z;
//
//			Vector3 targetDirection = (target.position - posNoZ);
//
//			interpVelocity = targetDirection.magnitude * 5f;
//
//			targetPos = myTransform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
//
//			myTransform.position = Vector3.Lerp (myTransform.position, targetPos + offset, time);
//		}
	}
}