using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;

    [Header("Spawn Timing")]
    public float baseSpawnDelay = 2f;
    public float minSpawnDelay = 0.3f;
    public float maxSpawnDelay = 3f;

    [Header("Random Offset")]
    public float baseRandomOffset = 1f;
    public float minRandomOffset = 0.1f;
    public float maxRandomOffset = 2f;

    [Header("Bullet Speed Scaling")]
    public float baseBulletSpeedBonus = 0f;
    public float bulletSpeedBonusPerDifficulty = 0.1f;
    public float maxBulletSpeedBonus = 5f;

    public SpawnControl spawnControl;

    private float spawnTimer;

    void Start()
    {
        ResetSpawnTimer();
    }

    void Update()
    {
        if (spawnControl != null && !spawnControl.AbleToSpawn)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            Spawn();
            ResetSpawnTimer();
        }
    }

    void ResetSpawnTimer()
    {
        float difficulty = spawnControl != null ? spawnControl.difficulty : 0f;

        float delay = Mathf.Clamp(baseSpawnDelay - difficulty * 1.5f, minSpawnDelay, maxSpawnDelay);
        float offset = Mathf.Clamp(baseRandomOffset - difficulty * 0.5f, minRandomOffset, maxRandomOffset);

        spawnTimer = delay + Random.Range(0f, offset);
    }

    void Spawn()
    {
        if (prefabsToSpawn.Length == 0) return;

        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);

        BulletEnemy bullet = instance.GetComponent<BulletEnemy>();
        if (bullet != null && spawnControl != null)
        {
            float bonus = Mathf.Clamp(
                baseBulletSpeedBonus + spawnControl.difficulty * bulletSpeedBonusPerDifficulty,
                baseBulletSpeedBonus,
                maxBulletSpeedBonus
            );

            bullet.speed += bonus;
        }
    }
}
