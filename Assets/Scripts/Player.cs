using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private float baseMovementSpeed = 5f;

	// internal components
	private Rigidbody2D myRigidbody;

	private SpriteRenderer mySpriteRenderer;

	private bool isGrounded;

	private bool isFacingRight = true;

	void Awake ()
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void Start ()
	{
	}

	void Update ()
	{
		MovementWithTouchScreen ();

		if (Debug.isDebugBuild) {
			MovementWithKeyboard ();

			//Debug.Log (CanAttack ());
		}

		UpdateFacing ();
	}

	void MovementWithTouchScreen ()
	{
		if (Input.touchCount < 1) {
			return;
		}

		Touch firstTouch = Input.GetTouch (0);
		float velocityX = 0f;

		if (firstTouch.position.x > Screen.width / 2) {
			velocityX = baseMovementSpeed;
		} else if (firstTouch.position.x < Screen.width / 2) {
			velocityX = baseMovementSpeed * -1;
		} else {
			velocityX = 0;
		}

		Debug.Log (velocityX);

		Move (new Vector2 (velocityX, myRigidbody.velocity.y));
	}

	void MovementWithKeyboard ()
	{
		float velocityX = 0f;

		if (Input.GetKey (KeyCode.RightArrow)) {
			velocityX = baseMovementSpeed;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			velocityX = baseMovementSpeed * -1;
		}

		Move (new Vector2 (velocityX, myRigidbody.velocity.y));
	}

	void Move (Vector2 velocity)
	{
		isGrounded = IsTouchingGroundLayer ();

		if (IsTouchingGroundLayer ()) {
			myRigidbody.velocity = velocity;
		}
	}

	bool IsTouchingGroundLayer ()
	{
		float hitLocation = 0.75f;
		float hitDuration = 0.4f;
		
		Vector2 hit1Position = new Vector2 (transform.position.x - hitLocation, transform.position.y);
		Vector2 hit2Position = new Vector2 (transform.position.x + hitLocation, transform.position.y);

		Debug.DrawRay (hit1Position, Vector2.down, Color.green, hitDuration);
		Debug.DrawRay (hit2Position, Vector2.down, Color.blue, hitDuration);

		RaycastHit2D hit1 = Physics2D.Raycast (hit1Position, Vector2.down, hitDuration);
		RaycastHit2D hit2 = Physics2D.Raycast (hit2Position, Vector2.down, hitDuration);

		bool hit1CollidesWithFloor = hit1.collider != null && hit1.collider.gameObject.layer == 8;
		bool hit2CollidesWithFloor = hit2.collider != null && hit2.collider.gameObject.layer == 8;

		return hit1CollidesWithFloor || hit2CollidesWithFloor;
	}

	bool CanAttack ()
	{
		float hitXLocation = 0.25f;
		float hitYLocation = 0.5f;
		float hitDuration = 1f;

		Vector2 hitPosition = new Vector2 (transform.position.x + hitXLocation, transform.position.y + hitYLocation);

		Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

		Debug.DrawRay (hitPosition, direction, Color.green, hitDuration / 2);

		RaycastHit2D hit = Physics2D.Raycast (hitPosition, direction, hitDuration);

		if (hit.collider != null) {
			Debug.Log (hit.collider.gameObject.layer);
		}

		return hit.collider != null && hit.collider.gameObject.layer == 9;
	}

	void UpdateFacing ()
	{
		mySpriteRenderer.flipX = !isFacingRight;
	}
}
