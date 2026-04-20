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

    public AudioClip shootSound;

    private float fireTimer = 0f;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (player == null) return;

        // Perseguir o player no eixo X
        float targetX = player.position.x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Rotacionar para apontar ao player
        // Subtrai 90 graus porque a sprite do canhão aponta para cima por padrão
        Vector2 aimDir = player.position - transform.position;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

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

        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);

        Vector2 direction = (player.position - transform.position).normalized;
        Vector3 spawnPos = transform.position + (Vector3)(direction * 0.6f);
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        float elapsedTime = GameController.GetTempoDecorrido();
        float currentProjectileSpeed = Mathf.Min(maxProjectileSpeed, projectileSpeed + elapsedTime * projectileSpeedIncreasePerSecond);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = direction * currentProjectileSpeed;
        }
    }
}
