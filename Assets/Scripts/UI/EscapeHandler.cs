using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeHandler : MonoBehaviour
{
    public bool returnToScene = false;
    public string sceneToLoad = "MainMenu";

    [Header("Menu Toggles")]
    public GameObject optionalCloseTarget;   // E.g., settings panel
    public GameObject pairedToggleTarget;    // E.g., main menu to hide/show

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeAction();
        }
    }

    // Call this from a UI Button to mimic Escape key behavior
    public void TriggerEscapeAction()
    {
        HandleEscapeAction();
    }

    private void HandleEscapeAction()
    {
        if (optionalCloseTarget != null && optionalCloseTarget.activeSelf)
        {
            CloseOptionalTarget();
            return;
        }

        if (returnToScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public void OpenOptionalTarget()
    {
        if (optionalCloseTarget != null)
            optionalCloseTarget.SetActive(true);

        if (pairedToggleTarget != null)
            pairedToggleTarget.SetActive(false);
    }

    public void CloseOptionalTarget()
    {
        if (optionalCloseTarget != null)
            optionalCloseTarget.SetActive(false);

        if (pairedToggleTarget != null)
            pairedToggleTarget.SetActive(true);
    }
}
