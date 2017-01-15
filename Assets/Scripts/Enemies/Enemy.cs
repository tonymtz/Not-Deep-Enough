using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	protected int healthPoints = 1;

	[SerializeField]
	protected float movementSpeed = 1;

	[SerializeField]
	protected bool isFacingRight = true;

	protected Rigidbody2D myRigidbody;

	protected void Awake ()
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	private void Update ()
	{
		CheckIfDead ();
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.layer == 11) {
			healthPoints -= 1;
		}
	}

	protected void CheckIfDead ()
	{
		if (healthPoints <= 0) {
			Destroy (gameObject);
		}
	}

	protected void Move ()
	{
		int direction = isFacingRight ? 1 : -1;
		Vector2 velocity = new Vector2 (movementSpeed * direction * Time.deltaTime, myRigidbody.velocity.y);

		myRigidbody.velocity = velocity;
	}

	protected void ToggleFacing ()
	{
		isFacingRight = !isFacingRight;

		if (isFacingRight) {
			transform.localScale = new Vector3 (1, 1, 1);
		} else {
			transform.localScale = new Vector3 (-1, 1, 1);
		}
	}
}
