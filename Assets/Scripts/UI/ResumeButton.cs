using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject targetToDeactivate;

    public void ResumeGame()
    {
        if (targetToDeactivate != null)
            targetToDeactivate.SetActive(false);

        Time.timeScale = 1f;
    }
}
