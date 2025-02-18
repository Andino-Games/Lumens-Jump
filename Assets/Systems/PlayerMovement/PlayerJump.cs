using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Config")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Revisar si está tocando el suelo usando Physics2D.Raycast
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayer);

        if (isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}