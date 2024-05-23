using UnityEngine;

public class AvoidEnemy : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

   
    void FixedUpdate()
    {
        // Move the player based on input
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            // Determine the direction away from the enemy
            Vector2 directionAwayFromEnemy = (rb.position - (Vector2)other.transform.position).normalized;

            // Move the player away from the enemy
            rb.MovePosition(rb.position + directionAwayFromEnemy * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
