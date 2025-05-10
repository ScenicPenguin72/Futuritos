using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AgarrarrCosas : MonoBehaviour
{
    public Slider slider;
    public float valuePerCorrect = 0.1f;

    [Header("Visual Feedback")]
    public GameObject correctFeedback;
    public GameObject incorrectFeedback;
    public float feedbackDuration = 0.15f;

    [Header("Game State Objects")]
    public GameObject spawner;
    public GameObject winObject;
    public GameObject gameOverObject;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float timerDuration = 60f;

    private float timerRemaining;
    private bool gameEnded = false;
    private Coroutine correctRoutine;
    private Coroutine incorrectRoutine;
    private MovimientoHorizontal Movement;
    private Color defaultColor = Color.black;
    private bool isBlinking = false;

    private void Start()
    {
        timerRemaining = timerDuration;
        Movement =  GameObject.FindGameObjectWithTag("Player").GetComponent<MovimientoHorizontal>();
    }

    private void Update()
    {
        if (gameEnded) return;

        UpdateTimer();

        if (slider.value >= 1f)
        {
            EndGame(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameEnded) return;

        if (collision.CompareTag("Correcto"))
        {
            ModifySlider(valuePerCorrect);
            ShowFeedback(ref correctRoutine, correctFeedback);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Incorrecto"))
        {
            ModifySlider(-1.7f * valuePerCorrect);
            ShowFeedback(ref incorrectRoutine, incorrectFeedback);
            Destroy(collision.gameObject);
        }
    }

    private void ModifySlider(float amount)
    {
        if (slider == null) return;
        slider.value = Mathf.Clamp(slider.value + amount, 0f, 1f);
    }

    private void ShowFeedback(ref Coroutine currentRoutine, GameObject feedbackObject)
    {
        if (feedbackObject == null) return;

        feedbackObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(DisableAfterDelay(feedbackObject, feedbackDuration));
    }

    private IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private void UpdateTimer()
    {
        if (timerRemaining > 0f)
        {
            timerRemaining -= Time.deltaTime;
            int totalSeconds = Mathf.CeilToInt(timerRemaining);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";

            if (totalSeconds <= 10)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = defaultColor;
            }

            if (totalSeconds <= 5 && !isBlinking)
            {
                StartCoroutine(BlinkText());
            }

            if (timerRemaining <= 0f)
            {
                EndGame(false);
            }
        }
    }

    private IEnumerator BlinkText()
    {
        isBlinking = true;
        while (timerRemaining > 0 && Mathf.CeilToInt(timerRemaining) <= 5)
        {
            timerText.enabled = false;
            yield return new WaitForSeconds(0.25f);
            timerText.enabled = true;
            yield return new WaitForSeconds(0.25f);
        }
        isBlinking = false;
    }


    private void EndGame(bool win)
    {
        gameEnded = true;

        if (spawner != null)
            spawner.SetActive(false);

        if (win)
        {
            if (winObject != null) winObject.SetActive(true);
        }
        else
        {
            if (gameOverObject != null) gameOverObject.SetActive(true);
        }

        Movement.enabled = false;
    }
}
