using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public float spawnDelay = 3f;
    public bool AbleToSpawn = true;

    [Header("Start Delay")]
    public float startDelay = 2f; // Delay after game starts before anything spawns
    private bool hasGameStarted = false;

    [Header("Difficulty Settings")]
    public float difficultyIncreaseRate = 1f;
    public float baseStartColorDuration = 2f;
    public float baseFinalColorDelay = 1f;
    public float minStartColorDuration = 0.5f;
    public float minFinalColorDelay = 0.2f;

    public float baseBulletSpeed = 5f;
    public float bulletSpeedIncreaseRate = 1f;
    public float maxBulletSpeed = 20f;

    [Header("Pre-spawn Block Time")]
    public float preSpawnBlockTime = 3f;

    private float timeSinceStart = 0f;
    private float spawnTimer = 0f;
    public float difficulty = 1;

    void Start()
    {
        AbleToSpawn = false;
        Invoke(nameof(StartGame), startDelay);
    }

    void StartGame()
    {
        hasGameStarted = true;
        AbleToSpawn = true;
    }

    void Update()
    {
        if (!hasGameStarted || !AbleToSpawn) return;

        spawnTimer += Time.deltaTime;
        timeSinceStart += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            spawnTimer = 0f;
            AbleToSpawn = false;
            Invoke(nameof(SpawnWithDelays), preSpawnBlockTime);
        }
    }

    void SpawnWithDelays()
    {
        difficulty = timeSinceStart * difficultyIncreaseRate;

        float currentStartDelay = Mathf.Min(minStartColorDuration, baseStartColorDuration + difficulty / 10);
        float currentFinalDelay = Mathf.Max(minFinalColorDelay, baseFinalColorDelay - difficulty / 5);
        float currentBulletSpeed = Mathf.Min(maxBulletSpeed, baseBulletSpeed + difficulty * bulletSpeedIncreaseRate);

        preSpawnBlockTime = Mathf.Min(2.2f, 3 - difficulty / 15);

        GameObject instance = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // Apply traffic light behavior
        ColorCycler cycler = instance.GetComponent<ColorCycler>();
        if (cycler != null)
        {
            cycler.switchTime = currentStartDelay;
            cycler.finalColorDelay = currentFinalDelay;
        }

        // Apply bullet difficulty
        BulletEnemy bullet = instance.GetComponentInChildren<BulletEnemy>();
        if (bullet != null)
        {
            bullet.speed = currentBulletSpeed;
        }

        float totalBlockTime = 1.5f;
        Invoke(nameof(EnableSpawning), totalBlockTime);
    }

    void EnableSpawning()
    {
        AbleToSpawn = true;
    }
}
