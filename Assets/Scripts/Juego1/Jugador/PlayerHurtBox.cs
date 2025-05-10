using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    public Vida vida;
    public int damagePerHit = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && vida != null && vida.IsAlive())
        {
            vida.TryTakeDamage(damagePerHit);
        }
    }
}
