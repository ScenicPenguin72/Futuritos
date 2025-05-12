using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RetiradaEnemiga : MonoBehaviour
{
    private Collider2D playerCollider;
    private HashSet<GameObject> triggeredEnemies = new HashSet<GameObject>();
    public float retriggerCooldown = 5f;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();

        if (playerCollider == null)
        {
            Debug.LogError("RetiradaEnemiga requires a Collider2D on the same GameObject.");
        }
    }

    public void TriggerRetreat()
    {
        if (playerCollider == null) return;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        filter.useLayerMask = true;

        Collider2D[] results = new Collider2D[20];
        int count = playerCollider.OverlapCollider(filter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col != null && col.CompareTag("Enemy") && col.isTrigger)
            {
                GameObject enemy = col.gameObject;

                if (!triggeredEnemies.Contains(enemy))
                {
                    Animator anim = enemy.GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetTrigger("Retirada");
                    }

                    BulletEnemy bullet = enemy.GetComponent<BulletEnemy>();
                    if (bullet != null)
                    {
                        bullet.speed = 0;
                    }

                    triggeredEnemies.Add(enemy);
                    StartCoroutine(RemoveAfterCooldown(enemy));
                }
            }
        }
    }

    private IEnumerator RemoveAfterCooldown(GameObject enemy)
    {
        yield return new WaitForSeconds(retriggerCooldown);
        triggeredEnemies.Remove(enemy);
    }
}
