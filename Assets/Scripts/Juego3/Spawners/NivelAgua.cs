using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NivelAgua : MonoBehaviour
{
    public static NivelAgua instance;

    [Header("UI Elements")]
    public Slider slider;
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    private float startingMinutes = 1f;
    private float startingSeconds = 35f;

    private float timeRemaining;
    private bool gameEnded = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        slider.value = 1f;
        timeRemaining = startingMinutes * 60f + startingSeconds;
        UpdateTimerText();
    }

    private void Update()
    {
        if (gameEnded) return;

        // Countdown
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0);
            UpdateTimerText();

            if (timeRemaining <= 0)
            {
                Victory();
            }
        }

        // Check slider
        if (slider.value <= 0f)
        {
            GameOver();
        }
    }

    public void ReduceSlider(float amount)
    {
        if (gameEnded) return;

        slider.value = Mathf.Clamp01(slider.value - amount);

        if (slider.value <= 0f)
        {
            GameOver();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void GameOver()
    {
        gameEnded = true;
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    private void Victory()
    {
        gameEnded = true;
        if (victoryUI != null)
            victoryUI.SetActive(true);
    }
}
