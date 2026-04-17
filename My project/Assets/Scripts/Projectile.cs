using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    private bool alreadyConsumed = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignora especificamente a área de spawn do CollectibleSpawner
        if (other.GetComponent<CollectibleSpawner>() != null)
            return;
        HandleHit(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    void HandleHit(GameObject otherObject)
    {
        if (alreadyConsumed)
        {
            return;
        }

        if (otherObject == null)
        {
            return;
        }

        if (otherObject.CompareTag("Player") || otherObject.GetComponentInParent<PlayerMove>() != null)
        {
            alreadyConsumed = true;
            GameController.PlayerHit();
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        if (!otherObject.CompareTag("cannon") && !otherObject.CompareTag("coletavel"))
        {
            alreadyConsumed = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
