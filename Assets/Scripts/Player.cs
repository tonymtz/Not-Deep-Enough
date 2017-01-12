using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float baseMovementSpeed = 5f;

    [SerializeField]
    private GameObject weapon;

    private Rigidbody2D myRigidbody;

    private SpriteRenderer mySpriteRenderer;

    private bool isGrounded;

    private bool isFacingRight = true;

    private bool canAttack;

    private bool isKeyLocked;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        if (weapon) { HideWeapon(); }
    }

    private void Update()
    {
        MovementWithTouchScreen();

        if (Debug.isDebugBuild) { MovementWithKeyboard(); }
    }

    private void MovementWithTouchScreen()
    {
        if (Input.touchCount == 0) { return; }

        Touch firstTouch = Input.GetTouch(0);

        if (firstTouch.phase == TouchPhase.Ended) { isKeyLocked = false; }

        if (isKeyLocked) { return; }

        if (firstTouch.position.x > Screen.width / 2) { RightClick(); }
        else if (firstTouch.position.x < Screen.width / 2) { LeftClick(); }
    }

    private void MovementWithKeyboard()
    {
        if (Input.GetKey(KeyCode.RightArrow)) { RightClick(); }
        else if (Input.GetKey(KeyCode.LeftArrow)) { LeftClick(); }
    }

    private void RightClick()
    {
        if (!isFacingRight) { ToggleFacing(); }
        Action();
    }

    private void LeftClick()
    {
        if (isFacingRight) { ToggleFacing(); }
        Action();
    }

    private void ToggleFacing()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight) { transform.localScale = new Vector3(1, 1, 1); }
        else { transform.localScale = new Vector3(-1, 1, 1); }
    }

    private void Action()
    {
        if (IsEnemyNearby())
        { Attack(); }
        else
        {
            int direction = isFacingRight ? 1 : -1;
            Move(new Vector2(baseMovementSpeed * direction, myRigidbody.velocity.y));
        }

    }

    private void Attack()
    {
        if (!canAttack) { return; }

        canAttack = false;
        isKeyLocked = true;
        weapon.SetActive(true);
        weapon.GetComponent<Animator>().SetTrigger("StartAttack");
        Invoke("HideWeapon", 0.25f);
    }

    private void HideWeapon()
    {
        weapon.SetActive(false);
        canAttack = true;
    }

    private void Move(Vector2 velocity)
    {
        isGrounded = IsTouchingGroundLayer();

        if (IsTouchingGroundLayer()) { myRigidbody.velocity = velocity; }
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
        float hitXLocation = 0.25f;
        float hitYLocation = 0.5f;
        float hitDuration = 1f;

        Vector2 hitPosition = new Vector2(transform.position.x + hitXLocation, transform.position.y + hitYLocation);

        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        Debug.DrawRay(hitPosition, direction, Color.green, hitDuration / 2);

        RaycastHit2D hit = Physics2D.Raycast(hitPosition, direction, hitDuration);

        return hit.collider != null;
    }
}
