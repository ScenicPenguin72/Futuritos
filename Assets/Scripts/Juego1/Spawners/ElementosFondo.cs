using System.Collections;
using UnityEngine;

public class ElementosFondo : MonoBehaviour
{
    [Header("Bush Appearance")]
public Color bushColor = Color.white;
    public GameObject[] buildingPrefabs;
    public GameObject[] bushPrefabs;
    public Transform spawnPoint;

    [Header("Spawn Settings")]
    public bool useDistanceInsteadOfTime = false;
    public bool debugSpawnInstant = false;
    public int debugSpawnCount = 5;
    public float spawnDelay = 1.0f;
    public float spawnDistance = 5.0f;

    [Header("Speed Settings")]
    public float baseSpeed = 2.0f;

    [Header("Layer Settings")]
    public int sortingLayerOffset = 0;

    [Header("Scale Settings")]
    public Vector3 scaleMultiplier = Vector3.one;

    [Header("Debug")]
    public int spawnedBuildingsCount = 0;

    private bool spawnBuildingNext = true;
    private float currentSpawnX;

    void Start()
    {
        currentSpawnX = spawnPoint.position.x;

        if (debugSpawnInstant)
        {
            SpawnMultiple(debugSpawnCount);
        }
        else if (useDistanceInsteadOfTime)
        {
            SpawnNextElementWithDistance(); 
        }
        else
        {
            StartCoroutine(SpawnTimedPattern());
        }
    }

    void Update()
    {
        if (useDistanceInsteadOfTime && !debugSpawnInstant)
        {
            float distance = Mathf.Abs(spawnPoint.position.x - currentSpawnX);
            if (distance <= 0.01f) return;

            if (distance >= spawnDistance)
            {
                SpawnNextElementWithDistance();
            }
        }
    }

    IEnumerator SpawnTimedPattern()
    {
        while (true)
        {
            SpawnNextElementAt(spawnPoint.position); 
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnMultiple(int buildingCount)
    {
        for (int i = 0; i < buildingCount; i++)
        {
            spawnBuildingNext = true;
            SpawnNextElementWithDistance();

            spawnBuildingNext = false;
            SpawnNextElementWithDistance();
        }
    }

    void SpawnNextElementWithDistance()
    {
        Vector3 spawnPos = new Vector3(currentSpawnX, spawnPoint.position.y, spawnPoint.position.z);
        SpawnNextElementAt(spawnPos);
        currentSpawnX += spawnDistance; 
    }

    void SpawnNextElementAt(Vector3 spawnPos)
{
    GameObject[] pool = spawnBuildingNext ? buildingPrefabs : bushPrefabs;
    GameObject prefab = pool[Random.Range(0, pool.Length)];

    GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

    // Movement
    Movimiento mover = obj.GetComponent<Movimiento>();
    if (mover != null)
    {
        mover.Inicializar(baseSpeed);
    }

    // Sprite Renderer
    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        if (!spawnBuildingNext) 
        {
            sr.color = bushColor;
        }

    
            sr.sortingOrder += sortingLayerOffset;
    
    }

    obj.transform.localScale = Vector3.Scale(obj.transform.localScale, scaleMultiplier);

    if (spawnBuildingNext)
    {
        spawnedBuildingsCount++;
    }
    spawnBuildingNext = !spawnBuildingNext;
}

}
