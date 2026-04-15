using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    
    public float speed;
    public AudioSource audioSource;
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement = movement.normalized; // Normaliza o vetor para evitar movimento mais rápido na diagonal

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        
       
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
