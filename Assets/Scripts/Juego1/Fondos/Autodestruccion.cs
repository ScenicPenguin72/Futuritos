using UnityEngine;

public class Autodestruccion : MonoBehaviour
{
    public float lifetime = 10f;

    void Start()
    {
        if (lifetime > 0f)
        {
            Destroy(gameObject, lifetime);
        }
    }
}
