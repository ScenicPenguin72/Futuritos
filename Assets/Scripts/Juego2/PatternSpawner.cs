using UnityEngine;
using System.Collections;
public class AnimationDrivenSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] goodItems;
    public GameObject[] badItems;
    [Range(0f, 1f)] public float badItemChance = 0.2f;

    [Header("Animator Random Triggers")]
    public Animator animator;
    public string[] triggerNames;
    public float minTriggerInterval = 2f;
    public float maxTriggerInterval = 5f;

    [Header("Dificultad Dinámica")]
    public float minBulletSpeed = 5f;
    public float maxBulletSpeed = 10f;
    public float minBadItemChance = 0.2f;
    public float maxBadItemChance = 0.4f;
    public float minAnimatorSpeed = 1f;
    public float maxAnimatorSpeed = 1.25f;
    public float tiempoParaMaximaDificultad = 60f;

    private float tiempoTranscurrido = 0f;
    private float dificultad = 0f;
    [Header("Secuencia de Aparición")]
    public float spawnDelay = 0.5f;
    [Header("Cooldown Settings")]
    public string cooldownTriggerName = "Cooldown"; // nombre del trigger en el Animator
    public float cooldownTime = 2f;


    private void Start()
{
    if (animator != null && triggerNames.Length > 0)
    {
        // Lanza el primer trigger aleatorio sin cooldown
        string firstTrigger = triggerNames[Random.Range(0, triggerNames.Length)];
        animator.SetTrigger(firstTrigger);

        // Programa las siguientes con cooldown
        Invoke(nameof(TriggerRandomAnimation), Random.Range(minTriggerInterval, maxTriggerInterval));
    }
}

    public void SpawnSequenceFromAnim(int itemType)
    {
        bool spawnBadItems = itemType == 1;
        StartCoroutine(SpawnItemsCoroutine(spawnBadItems));
    }

    private IEnumerator SpawnItemsCoroutine(bool spawnBadItems)
    {
        Vector3 posicionInicial = transform.position;

        for (int i = 0; i < 4; i++)
        {
            GameObject prefab = spawnBadItems ? GetRandom(badItems) : GetRandom(goodItems);
            GameObject obj = Instantiate(prefab, posicionInicial, Quaternion.identity);

            BulletEnemy bullet = obj.GetComponent<BulletEnemy>();
            if (bullet != null)
            {
                float velocidadBala = Mathf.Lerp(minBulletSpeed, maxBulletSpeed, dificultad);
                bullet.speed = velocidadBala;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        // Aumentar dificultad linealmente hasta 1
        tiempoTranscurrido += Time.deltaTime;
        dificultad = Mathf.Clamp01(tiempoTranscurrido / tiempoParaMaximaDificultad);

        // Ajustar velocidad del Animator
        if (animator != null)
        {
            animator.speed = Mathf.Lerp(minAnimatorSpeed, maxAnimatorSpeed, dificultad);
        }

        // Ajustar chance de ítems malos
        badItemChance = Mathf.Lerp(minBadItemChance, maxBadItemChance, dificultad);
    }

    public void SpawnRandomItem()
    {
        GameObject prefab = Random.value < badItemChance ? GetRandom(badItems) : GetRandom(goodItems);
        Spawn(prefab);
    }

    public void SpawnOnlyGoodItem()
    {
        GameObject prefab = GetRandom(goodItems);
        Spawn(prefab);
    }

    public void SpawnOnlyBadItem()
    {
        GameObject prefab = GetRandom(badItems);
        Spawn(prefab);
    }

    public void RandomSpawnChance()
    {
        if (Random.Range(0, 20) == 0)
        {
            SpawnRandomItem();
        }
    }

    private void Spawn(GameObject prefab)
    {
        if (prefab == null) return;

        float velocidadBala = Mathf.Lerp(minBulletSpeed, maxBulletSpeed, dificultad);

        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        BulletEnemy bullet = obj.GetComponent<BulletEnemy>();
        if (bullet != null)
        {
            bullet.speed = velocidadBala;
        }
    }

    private GameObject GetRandom(GameObject[] array)
    {
        if (array == null || array.Length == 0) return null;
        return array[Random.Range(0, array.Length)];
    }

    private void TriggerRandomAnimation()
    {
        if (animator != null && triggerNames.Length > 0)
        {
            string selectedTrigger = triggerNames[Random.Range(0, triggerNames.Length)];
            StartCoroutine(TriggerAnimationWithCooldown(selectedTrigger));
        }
    }
    private IEnumerator TriggerAnimationWithCooldown(string nextTrigger)
    {
        // Activa primero la animación de cooldown
        animator.SetTrigger(cooldownTriggerName);

        // Espera el tiempo de cooldown
        yield return new WaitForSeconds(cooldownTime);

        // Luego activa la animación deseada
        animator.SetTrigger(nextTrigger);

        // Programa la siguiente animación aleatoria
        float intervalo = Random.Range(minTriggerInterval, maxTriggerInterval);
        Invoke(nameof(TriggerRandomAnimation), intervalo);
    }

}
