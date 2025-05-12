using UnityEngine;
using System.Collections.Generic;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float baseFireRate = 0.5f;
    public float minFireRate = 0.1f;
    public float bulletOffsetDistance = 1f;
    public float circularFireAngleStep = 15f;
    public float spiralRotationSpeed = 90f;

    public float baseBulletSpeed = 5f;
    public float maxBulletSpeed = 20f;

    public float difficultyRate = 1f;

    public enum FireMode { Straight, Circular }
    public FireMode currentMode = FireMode.Straight;

    public bool multiFireEnabled = false;

    public List<Transform> firePoints;
    public Transform CircularOrigin;

    public SpawnControl spawnControl;

    private float fireTimer;
    private float modeTimer = 0f;
    private float modeSwitchInterval = 5f;

    private float difficultyTime = 0f;

    void Update()
    {
        if (spawnControl != null && !spawnControl.AbleToSpawn)
            return;

        difficultyTime += Time.deltaTime;
        float difficulty = spawnControl.difficulty;
        float fireRate = Mathf.Max(minFireRate, baseFireRate - difficulty);
        fireTimer += Time.deltaTime;
        modeTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Shoot(difficulty);
        }
        float adjustedModeInterval = Mathf.Max(1f, modeSwitchInterval - difficulty * difficultyRate);

        if (modeTimer >= adjustedModeInterval)
        {
            modeTimer = 0f;
            currentMode = (FireMode)(((int)currentMode + 1) % 2);
        }
    }

    void Shoot(float difficulty)
    {
        if (multiFireEnabled && firePoints.Count > 0)
        {
            foreach (Transform firePoint in firePoints)
            {
                FireFromPoint(firePoint, difficulty);
            }
        }
        else
        {
            FireFromPoint(transform, difficulty);
        }
    }

    void FireFromPoint(Transform shootOrigin, float difficulty)
    {
        switch (currentMode)
        {
            case FireMode.Straight:
                SpawnBullet(shootOrigin.up, shootOrigin.position, difficulty);
                break;
            case FireMode.Circular:
                float baseAngle = CircularOrigin ? CircularOrigin.eulerAngles.z : shootOrigin.eulerAngles.z;
                for (float angle = 0; angle < 360; angle += circularFireAngleStep)
                {
                    float totalAngle = baseAngle + angle;
                    Vector2 dir = Quaternion.Euler(0, 0, totalAngle) * Vector2.up;
                    SpawnBullet(dir, shootOrigin.position, difficulty);
                }
                break;
            
        }
    }

    void SpawnBullet(Vector2 direction, Vector3 origin, float difficulty)
    {
        Vector3 spawnPos = multiFireEnabled ? origin : origin + (Vector3)(direction.normalized * bulletOffsetDistance);

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        BulletEnemy b = bullet.GetComponent<BulletEnemy>();

        b.direction = direction;
        b.originPoint = spawnPos;
        b.usePathMotion = false;
        b.curveRight = false;
        b.curveRadiusSpeed = 1.5f;
        b.curveRotationSpeed = 90;
        b.speed = Mathf.Min(maxBulletSpeed, baseBulletSpeed + difficulty*1.35f);
    }

}
