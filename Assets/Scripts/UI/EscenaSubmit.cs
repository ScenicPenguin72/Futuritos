using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaSubmit : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad = "MainMenu"; // Name of the scene you want to load
    public bool sceneTimeChange = false;    // If true, scene will auto-load after 5 seconds

    private float timer = 0f;
    public float wait = 5f;
    private bool sceneLoaded = false;

    void Update()
    {
        // Handle input-based scene load
        if (!sceneLoaded && (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Escape)))
        {
            LoadScene();
        }

        // Handle timed scene load
        if (sceneTimeChange && !sceneLoaded)
        {
            timer += Time.deltaTime;

            if (timer >= wait)
            {
                LoadScene();
            }
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            sceneLoaded = true;
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene name specified to load.");
        }
    }
}
