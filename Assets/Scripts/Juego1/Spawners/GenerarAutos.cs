using System.Collections;
using UnityEngine;

public class GenerarAutos : MonoBehaviour
{
    public GameObject[] carPrefabs;            // Set your car prefabs here
    public Vector2 spawnPlace = new Vector2(22.5f, 5);
    private float spawnDelay = 0.3f;

    void OnEnable()
    {
        StartCoroutine(SpawnCars());
    }

    IEnumerator SpawnCars()
    {
        for (int i = 0; i < 3; i++)
        {

            GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Length)];
            Instantiate(prefab, (Vector3)spawnPlace, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }

        enabled = false; // optional: disable the script after use
    }
}
