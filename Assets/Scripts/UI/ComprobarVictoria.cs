using UnityEngine;
using UnityEngine.UI;

public class ComprobarVictoria : MonoBehaviour
{
    public Image uiImage;         // Reference to the UI Image
    public Button uiButton;       // Reference to the UI Button
    public string victoria;

    private void Start()
    {
        // Check if the PlayerPrefs value for "VictoryGame1" is 1 (true)
        if (PlayerPrefs.GetInt(victoria, 0) == 1)
        {
            // Set the UI Image color to white
            if (uiImage != null)
                uiImage.color = Color.white;

            // Make the button interactable
            if (uiButton != null)
                uiButton.interactable = true;
        }
    }
}
