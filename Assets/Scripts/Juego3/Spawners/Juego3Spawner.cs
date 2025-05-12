using UnityEngine;

public class Juego3Spawner : MonoBehaviour
{
    [Header("Spawn Area")]
    public Vector3 corner1;
    public Vector3 corner2;

    [Header("Prefab")]
    public GameObject prefabToSpawn;

    [Header("Spawn Timing")]
    public float baseSpawnDelay = 2f;
    public float minSpawnDelay = 0.3f;

    [Header("Random Offset")]
    public float baseRandomOffset = 1f;
    public float minRandomOffset = 0.1f;
    public float maxRandomOffset = 2f;

    [Header("Optional Control")]
    public SpawnControl spawnControl;

    private float spawnTimer;
    private float elapsedTime = 0f;
    private const float difficultyDuration = 60f; 

    void Start()
    {
        if (spawnControl == null || spawnControl.AbleToSpawn)
        {
            Spawn();
        }

        ResetSpawnTimer();
    }

    void Update()
    {
        if (spawnControl != null && !spawnControl.AbleToSpawn)
            return;

        elapsedTime += Time.deltaTime;
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            Spawn();
            ResetSpawnTimer();
        }
    }

    void ResetSpawnTimer()
    {
        float difficulty = Mathf.Clamp01(elapsedTime / difficultyDuration); 
        float adjustedDelay = Mathf.Lerp(baseSpawnDelay, minSpawnDelay, difficulty);
        float offset = Mathf.Clamp(baseRandomOffset, minRandomOffset, maxRandomOffset);

        spawnTimer = adjustedDelay + Random.Range(0f, offset);
    }

    void Spawn()
    {
        if (prefabToSpawn == null) return;

        Vector3 spawnPosition = new Vector3(
            Random.Range(Mathf.Min(corner1.x, corner2.x), Mathf.Max(corner1.x, corner2.x)),
            Random.Range(Mathf.Min(corner1.y, corner2.y), Mathf.Max(corner1.y, corner2.y)),
            Random.Range(Mathf.Min(corner1.z, corner2.z), Mathf.Max(corner1.z, corner2.z))
        );

        GameObject instance = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        if (Random.value < 0.5f)
        {
            Vector3 localScale = instance.transform.localScale;
            localScale.x *= -1;
            instance.transform.localScale = localScale;
        }
    }
}
