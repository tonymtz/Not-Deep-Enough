using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int healthPoints = 1;

    private void Awake()
    {
    }

    private void Update()
    {
        CheckIfDead();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            healthPoints -= 1;
        }
    }

    private void CheckIfDead()
    {
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
