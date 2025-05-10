using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
     public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Make sure time is resumed before loading
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
        #else
            Application.Quit(); // Quit the built app
        #endif
    }

}
