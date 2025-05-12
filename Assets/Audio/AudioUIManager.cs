using UnityEngine;
using UnityEngine.UI;

public class AudioUIManager : MonoBehaviour
{
    public GameObject Pausa;
    private Slider audioSlider;
    private Slider musicSlider;

    private const string AudioVolumePref = "AudioVolume";
    private const string MusicVolumePref = "MusicVolume";
    [SerializeField] private float maxVolume = 0.7f; 

    private void Awake()
    {
        Pausa.SetActive(true);
        GameObject audioSliderObject = GameObject.FindGameObjectWithTag("Audio");
        GameObject musicSliderObject = GameObject.FindGameObjectWithTag("Music");

        if (audioSliderObject != null)
        {
            audioSlider = audioSliderObject.GetComponent<Slider>();
            audioSlider.onValueChanged.AddListener(SetAudioVolume);
        }
        else
        {
            Debug.LogWarning("Audio slider not found! Make sure it's tagged as 'Audio'.");
        }

        if (musicSliderObject != null)
        {
            musicSlider = musicSliderObject.GetComponent<Slider>();
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else
        {
            Debug.LogWarning("Music slider not found! Make sure it's tagged as 'Music'.");
        }

        float savedAudioVolume = PlayerPrefs.GetFloat(AudioVolumePref, 0.5f); 
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumePref, 0.5f); 

        if (audioSlider != null)
        {
            audioSlider.value = savedAudioVolume;
            SetAudioVolume(savedAudioVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
            SetMusicVolume(savedMusicVolume);
        }

    }
    private void Start()
    {
        Pausa.SetActive(false);
    }

    private void SetAudioVolume(float sliderValue)
    {
        if (AudioManager.instance != null && AudioManager.instance.soundEffectsSource != null)
        {
            float scaledVolume = sliderValue * sliderValue * maxVolume; 
            AudioManager.instance.soundEffectsSource.volume = scaledVolume;
            PlayerPrefs.SetFloat(AudioVolumePref, sliderValue); 
        }
    }

    private void SetMusicVolume(float sliderValue)
    {
        if (AudioManager.instance != null && AudioManager.instance.musicSource != null)
        {
            float scaledVolume = sliderValue * sliderValue * maxVolume; 
            AudioManager.instance.musicSource.volume = scaledVolume;
            PlayerPrefs.SetFloat(MusicVolumePref, sliderValue); 
        }
    }
}
