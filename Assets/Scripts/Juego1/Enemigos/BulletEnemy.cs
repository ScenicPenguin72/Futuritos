using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;
    
    public bool useWave = false;
    public float waveFrequency = 5f;
    public float waveSharpness = 0.5f;

    public bool usePathMotion = false;
    public bool curveRight = true; 
    public float curveRadiusSpeed = 1f;
    public float curveRotationSpeed = 180f;

    public Vector2 originPoint;

    private float timeAlive = 0f;
    private Vector2 waveAxis;

    void Start()
    {
        direction.Normalize();
        waveAxis = new Vector2(-direction.y, direction.x);
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        if (usePathMotion)
        {
             float radius = timeAlive * curveRadiusSpeed;
        float angle = timeAlive * curveRotationSpeed * (curveRight ? 1f : -1f);

        Vector2 offset = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * radius;

        transform.position = originPoint + offset;
        return;
        }

        Vector2 finalMove = direction;

        if (useWave)
        {
            float waveOffset = Mathf.Sin(timeAlive * waveFrequency * Mathf.PI * 2) * waveSharpness;
            finalMove += waveAxis * waveOffset;
        }

        transform.position += (Vector3)(finalMove.normalized * speed * Time.deltaTime);
    }
}
