using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float baseMovementSpeed = 5f;

    [SerializeField]
    private GameObject weapon;

    private Rigidbody2D myRigidbody;

    private SpriteRenderer mySpriteRenderer;

	private Animator myAnimator;

    private bool isGrounded;

    private bool isFacingRight = true;

    private bool canAttack = true;

    private bool isKeyLocked;

	private bool isRunning;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
		myAnimator = GetComponent<Animator>();

        if (weapon) { HideWeapon(); }
    }

    private void Update()
    {
		isRunning = false;
        
		MovementWithTouchScreen();

        if (Debug.isDebugBuild) { MovementWithKeyboard(); }

		if (isRunning != myAnimator.GetBool ("IsRunning")) {
			myAnimator.SetBool ("IsRunning", isRunning);
		}

		if (!IsTouchingGroundLayer ()) {
			Vector2 velocity = new Vector2 (0f, myRigidbody.velocity.y);
			myRigidbody.velocity = velocity;
		}
    }

    private void MovementWithTouchScreen()
    {
        if (Input.touchCount == 0) { return; }

        Touch firstTouch = Input.GetTouch(0);

        bool isTouchOnRightSizeOfScreen = firstTouch.position.x > Screen.width / 2;

        if (firstTouch.phase == TouchPhase.Ended)
        {
            isKeyLocked = false;
        }
        else if (firstTouch.phase == TouchPhase.Began)
        {
            if (isTouchOnRightSizeOfScreen)
            {
                if (!isFacingRight) { ToggleFacing(); }
            }
            else
            {
                if (isFacingRight) { ToggleFacing(); }
            }

            if (IsEnemyNearby())
            {
                isKeyLocked = true;
                Attack();
            }
            else
            {
				Move ();
            }
        }
        else if (firstTouch.phase == TouchPhase.Stationary)
        {
            if (!isKeyLocked)
            {
				if (isTouchOnRightSizeOfScreen)
				{
					if (!isFacingRight) { ToggleFacing(); }
				}
				else
				{
					if (isFacingRight) { ToggleFacing(); }
				}

				Move ();
            }
        }
    }

    private void MovementWithKeyboard()
    {
		if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow)) {
			isKeyLocked = false;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {
			if (Input.GetKey (KeyCode.RightArrow)) {
				if (!isFacingRight) { ToggleFacing (); }
			} else {
				if (isFacingRight) { ToggleFacing(); }
			}

			if (IsEnemyNearby())
			{
				isKeyLocked = true;
				Attack();
			}
			else
			{
				Move ();
			}
		} else if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow)) {
			if (!isKeyLocked)
			{
				if (Input.GetKey (KeyCode.RightArrow)) {
					if (!isFacingRight) { ToggleFacing (); }
				} else {
					if (isFacingRight) { ToggleFacing(); }
				}

				Move ();
			}
		}
    }

    private void ToggleFacing()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight) { transform.localScale = new Vector3(1, 1, 1); }
        else { transform.localScale = new Vector3(-1, 1, 1); }
    }

    private void Attack()
    {
        if (!canAttack) { return; }

        canAttack = false;
        isKeyLocked = true;
        weapon.SetActive(true);
        weapon.GetComponent<Animator>().SetTrigger("StartAttack");
		myAnimator.SetTrigger("StartAttack");
        Invoke("HideWeapon", 0.4f);
    }

    private void HideWeapon()
    {
        weapon.SetActive(false);
        canAttack = true;
    }

    private void Move()
    {
        isGrounded = IsTouchingGroundLayer();

		if (!isGrounded) { return; }

		int direction = isFacingRight ? 1 : -1;
		Vector2 velocity = new Vector2 (baseMovementSpeed * direction * Time.deltaTime, myRigidbody.velocity.y);

		myRigidbody.velocity = velocity;

		isRunning = true;
    }

    private bool IsTouchingGroundLayer()
    {
        float hitLocation = 0.75f;
        float hitDuration = 0.4f;

        Vector2 hit1Position = new Vector2(transform.position.x - hitLocation, transform.position.y);
        Vector2 hit2Position = new Vector2(transform.position.x + hitLocation, transform.position.y);

        Debug.DrawRay(hit1Position, Vector2.down, Color.green, hitDuration);
        Debug.DrawRay(hit2Position, Vector2.down, Color.blue, hitDuration);

        RaycastHit2D hit1 = Physics2D.Raycast(hit1Position, Vector2.down, hitDuration);
        RaycastHit2D hit2 = Physics2D.Raycast(hit2Position, Vector2.down, hitDuration);

        bool hit1CollidesWithFloor = hit1.collider != null && hit1.collider.gameObject.layer == 8;
        bool hit2CollidesWithFloor = hit2.collider != null && hit2.collider.gameObject.layer == 8;

        return hit1CollidesWithFloor || hit2CollidesWithFloor;
    }

    private bool IsEnemyNearby()
    {
        float hitYLocation = 0.5f;
        float hitDuration = 1.5f;

        Vector2 hitPosition = new Vector2(transform.position.x, transform.position.y + hitYLocation);

        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        Debug.DrawRay(hitPosition, direction, Color.green, hitDuration / 2);

        RaycastHit2D hit = Physics2D.Raycast(hitPosition, direction, hitDuration);

		return hit.collider != null && hit.collider.gameObject.layer == 9;
    }
}
