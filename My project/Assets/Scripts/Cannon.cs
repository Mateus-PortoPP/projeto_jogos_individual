using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;

    public float moveSpeed = 2f;
    public float fireRate = 2f;
    public float projectileSpeed = 6f;

    public float minFireRate = 0.5f;
    public float fireRateDecreasePerSecond = 0.03f;
    public float projectileSpeedIncreasePerSecond = 0.25f;
    public float maxProjectileSpeed = 15f;

    private float fireTimer = 0f;

    void Update()
    {
        if (player == null) return;

        // Perseguir o player no eixo X
        float targetX = player.position.x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Aumenta a dificuldade conforme o tempo passa
        float elapsedTime = GameController.GetTempoDecorrido();
        float currentFireRate = Mathf.Max(minFireRate, fireRate - elapsedTime * fireRateDecreasePerSecond);

        // Atirar em direção ao player
        fireTimer += Time.deltaTime;
        if (fireTimer >= currentFireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || player == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        float elapsedTime = GameController.GetTempoDecorrido();
        float currentProjectileSpeed = Mathf.Min(maxProjectileSpeed, projectileSpeed + elapsedTime * projectileSpeedIncreasePerSecond);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * currentProjectileSpeed;
    }
}
