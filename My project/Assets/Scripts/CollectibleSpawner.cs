using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CollectibleSpawner : MonoBehaviour
{
    public static CollectibleSpawner Instance { get; private set; }

    public GameObject collectiblePrefab;
    public BoxCollider2D spawnArea;
    public int initialWaveCount = 2;
    public int waveIncrement = 2;
    public float nextWaveDelay = 1f;
    public float spawnCheckRadius = 0.25f;
    public int maxSpawnAttemptsPerCollectible = 20;

    private int currentWaveTarget;
    private int activeCollectibles;
    private int collectedThisWave;
    private bool waitingNextWave;
    private float nextWaveTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Use o collider do próprio objeto se nenhum foi definido no Inspector.
        if (spawnArea == null)
        {
            spawnArea = GetComponent<BoxCollider2D>();
        }

        // A área de spawn deve ser trigger para não bloquear física da cena.
        if (spawnArea != null)
        {
            spawnArea.isTrigger = true;
        }
    }

    void Start()
    {
        currentWaveTarget = Mathf.Max(1, initialWaveCount);
        SpawnWave(currentWaveTarget);
    }

    void Update()
    {
        if (waitingNextWave && Time.time >= nextWaveTime)
        {
            waitingNextWave = false;
            // Sempre spawna exatamente initialWaveCount (regra: aparecer só 2 por vez)
            SpawnWave(initialWaveCount);
        }
    }

    public void OnCollectibleCollected(GameObject collectedObject)
    {
        collectedThisWave++;
        activeCollectibles = Mathf.Max(0, activeCollectibles - 1);

        // Valida com contagem real da cena (o objeto ainda existe, por isso -1)
        int actualCount = Mathf.Max(0, CountSceneCollectibles() - 1);
        if (actualCount < activeCollectibles)
            activeCollectibles = actualCount;

        if (activeCollectibles == 0 && !waitingNextWave)
        {
            waitingNextWave = true;
            nextWaveTime = Time.time + nextWaveDelay;
        }
    }

    void SpawnWave(int amount)
    {
        if (collectiblePrefab == null)
        {
            return;
        }

        // Garante que não existam coletáveis sobrando na cena antes de spawnar novos
        if (CountSceneCollectibles() > 0)
        {
            waitingNextWave = true;
            nextWaveTime = Time.time + nextWaveDelay;
            return;
        }

        int spawned = 0;
        for (int i = 0; i < amount; i++)
        {
            if (SpawnOneCollectible())
            {
                spawned++;
            }
        }

        activeCollectibles = spawned;
        collectedThisWave = 0;

        // Se não conseguiu spawnar nada, tenta de novo depois.
        if (activeCollectibles == 0 && !waitingNextWave)
        {
            waitingNextWave = true;
            nextWaveTime = Time.time + nextWaveDelay;
        }
    }

    bool SpawnOneCollectible()
    {
        if (collectiblePrefab == null)
        {
            return false;
        }

        for (int attempt = 0; attempt < maxSpawnAttemptsPerCollectible; attempt++)
        {
            Vector2 position = GetRandomSpawnPosition();
            Collider2D overlap = Physics2D.OverlapCircle(position, spawnCheckRadius);

            // Só evita spawn em cima de outro coletável; colisões de cenário são esperadas.
            if (overlap != null && overlap.CompareTag("coletavel"))
            {
                continue;
            }

            GameObject spawned = Instantiate(collectiblePrefab, position, Quaternion.identity);
            spawned.tag = "coletavel";
            return true;
        }

        return false;
    }

    Vector2 GetRandomSpawnPosition()
    {
        if (spawnArea != null)
        {
            Bounds bounds = spawnArea.bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            return new Vector2(x, y);
        }

        Vector2 center = transform.position;
        float xFallback = Random.Range(center.x - 4f, center.x + 4f);
        float yFallback = Random.Range(center.y - 2f, center.y + 2f);
        return new Vector2(xFallback, yFallback);
    }

    int CountSceneCollectibles()
    {
        GameObject[] tagged = GameObject.FindGameObjectsWithTag("coletavel");
        int count = 0;

        foreach (GameObject go in tagged)
        {
            if (go == null)
            {
                continue;
            }

            // Evita contar objetos de sistema que receberam tag por engano.
            if (go.GetComponent<CollectibleSpawner>() != null)
            {
                continue;
            }

            if (go.GetComponent<Collider2D>() == null)
            {
                continue;
            }

            count++;
        }

        return count;
    }
}
