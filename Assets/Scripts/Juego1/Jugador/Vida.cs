 using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Vida : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Heart Settings")]
    public List<Image> heartIcons;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    [Header("Survival Timer")]
    public Slider survivalSlider;
    public float fillTime = 85f;
    private float survivalProgress = 0f;

    [Header("Invulnerability")]
    public float invulnerabilityDuration = 2f;
    public float blinkSpeed = 10f;
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    [Header("Game Object Control")]
    public GameObject spawnerObject;
    private bool spawnerDisabled = false;
    [Header("Game States")]
    public GameObject gameOverObject;
    public GameObject victoryObject;
    private bool victoryTriggered = false;

    private SpriteRenderer spriteRenderer;
    private Animator VidaAnim;
    private Color originalColor;
    private bool isDead = false;
    private bool isFadingOut = false;
    private float fadeTimer = 0f;
    private float fadeDuration = 1f;
    private Vector3 deathStartPosition;
    private Vector3 deathEndPosition;
    private AnimationCurve deathCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AudioClip hurt, gameover, victory;


    void Start()
    {
        currentHealth = maxHealth;
        VidaAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (survivalSlider != null)
        {
            survivalSlider.minValue = 0f;
            survivalSlider.maxValue = 1f;
            survivalSlider.value = 0f;
        }

        UpdateHeartIcons();
    }

    void Update()
    {
        if (isFadingOut)
        {
            fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);

            // Fade out
            float alpha = Mathf.Lerp(1f, 0f, t);
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            // Move along curve to bottom-left
            float curvedT = deathCurve.Evaluate(t);
            transform.position = Vector3.Lerp(deathStartPosition, deathEndPosition, curvedT);

            if (t >= 1f)
            {
                isFadingOut = false;
                gameObject.SetActive(false);
            }
        }


        if (IsAlive() && survivalSlider != null)
        {
            survivalProgress += Time.deltaTime / fillTime;
            survivalSlider.value = Mathf.Clamp01(survivalProgress);
        }
        if (!victoryTriggered && survivalSlider.value >= 1f)
        {
                victoryTriggered = true;
                if (victoryObject != null)
                {
                    victoryObject.SetActive(true);
                    isInvulnerable=true;
                }
        }


        if (!IsAlive() && !spawnerDisabled)
        {
            DisableSpawner();
        }

        float disableTime = (fillTime - 5f) / fillTime;
        if (survivalSlider != null && survivalSlider.value >= disableTime && !spawnerDisabled)
        {
            DisableSpawner();
        }

        // INVULNERABILIDAD Y PARPADEO
        if (isInvulnerable && !isDead)
        {
            invulnerabilityTimer -= Time.deltaTime;

            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            BlinkHeartIcons(alpha);

            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
                if (spriteRenderer != null)
                    spriteRenderer.color = originalColor;

                ResetHeartIconAlpha();
            }
        }
    }

    void DisableSpawner()
    {
        if (spawnerObject != null)
        {
            spawnerObject.SetActive(false);
            spawnerDisabled = true;
        }
    }

    public void TryTakeDamage(int amount)
    {
        if (isInvulnerable || !IsAlive()) return;

        TakeDamage(amount);
        BecomeInvulnerable();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        AudioManager.instance.PlaySoundEffect(hurt, 0.3f);
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHeartIcons();

        if (currentHealth == 0 && !isDead)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        isDead = true;
        isInvulnerable = false;

       if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
            isFadingOut = true;
            deathStartPosition = transform.position;
            deathEndPosition = deathStartPosition + new Vector3(-5.5f, -5.5f, 0f); // Adjust this for a more dramatic move

            fadeTimer = 0f;
        }

        ResetHeartIconAlpha();

        if (VidaAnim != null)
        {
            VidaAnim.SetTrigger("Muerto");
        }
        if (gameOverObject != null)
        {
            gameOverObject.SetActive(true);
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHeartIcons();
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public void ResetSurvivalProgress()
    {
        survivalProgress = 0f;
        if (survivalSlider != null)
        {
            survivalSlider.value = 0f;
        }
    }

    private void BecomeInvulnerable()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    private void UpdateHeartIcons()
    {
        int totalHearts = heartIcons.Count;
        int heartsToFill = Mathf.CeilToInt(((float)currentHealth / maxHealth) * totalHearts);

        for (int i = 0; i < totalHearts; i++)
        {
            heartIcons[i].sprite = i < heartsToFill ? fullHeartSprite : emptyHeartSprite;
        }
    }

    private void BlinkHeartIcons(float alpha)
    {
        foreach (var heart in heartIcons)
        {
            Color c = heart.color;
            c.a = alpha;
            heart.color = c;
        }
    }

    private void ResetHeartIconAlpha()
    {
        foreach (var heart in heartIcons)
        {
            Color c = heart.color;
            c.a = 1f;
            heart.color = c;
        }
    }
}