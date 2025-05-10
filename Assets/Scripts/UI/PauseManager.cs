using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject victoryUI;

    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            // Only toggle pause if neither end screen is active
            if ((gameOverUI == null || !gameOverUI.activeSelf) &&
                (victoryUI == null || !victoryUI.activeSelf))
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Make sure time is resumed before loading
        SceneManager.LoadScene(sceneName);
    }
}
