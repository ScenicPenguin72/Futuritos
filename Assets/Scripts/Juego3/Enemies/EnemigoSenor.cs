using UnityEngine;

public class EnemigoSenor : MonoBehaviour
{
    public float drainAmount = 0.1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Agua") && NivelAgua.instance != null)
        {
            NivelAgua.instance.ReduceSlider(drainAmount);
            Animator animator = GetComponent<Animator>();
            Stop();    
            if (animator != null)
            {
                animator.SetTrigger("Retirada");
            }   
        }
    }
    public void Stop()
    {
        BulletEnemy bullet = GetComponent<BulletEnemy>();
        bullet.speed = 0;
        
    }
}
