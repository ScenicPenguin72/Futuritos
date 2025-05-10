using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float velocidad;

    public void Inicializar(float nuevaVelocidad)
    {
        velocidad = velocidad + nuevaVelocidad;
    }

    void Update()
    {
        transform.position += Vector3.left * velocidad * Time.deltaTime;
    }
}
