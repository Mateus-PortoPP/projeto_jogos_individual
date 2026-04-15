using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;
    public float jumpForce = 10f;
    public AudioSource audioSource;

    private bool jumpRequested = false;
    private int jumpsRemaining = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (jumpsRemaining > 0)
                jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
            jumpRequested = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        jumpsRemaining = 2;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="coletavel")
        {
            audioSource.Play();
            
            GameController.Collect();
           
            Destroy(other.gameObject);
        }
    }
}
