using UnityEngine;

public class SetVictoryPref : MonoBehaviour
{
    public string Juego;
    private void OnEnable()
    {
        PlayerPrefs.SetInt(Juego, 1);
        PlayerPrefs.Save();  
    }
}
