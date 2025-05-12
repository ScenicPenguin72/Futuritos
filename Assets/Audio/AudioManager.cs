using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource soundEffectsSource;   
    public AudioSource musicSource;        

    [Header("Music Clips")]
    public AudioClip menuMusicClip;        
    public AudioClip gameMusicClip;          

    private const string AudioVolumePref = "AudioVolume";
    private const string MusicVolumePref = "MusicVolume";
    private AudioClip lastPlayedClip; 

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        float savedAudioVolume = PlayerPrefs.GetFloat(AudioVolumePref, 0.5f); 
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumePref, 0.5f); 

        soundEffectsSource.volume = savedAudioVolume;
        musicSource.volume = savedMusicVolume;

        PlayMusicBasedOnPlayerPresence();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;                  
    }
    private void PlayMusicBasedOnPlayerPresence()
    {  
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            PlayMusic(gameMusicClip, musicSource.volume);
        }
        else
        {
            PlayMusic(menuMusicClip, musicSource.volume);
        }
    }
    public void PlaySoundEffect(AudioClip clip, float volume = 1f)
    {
        soundEffectsSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip musicClip, float volume = 1f, float fadeDuration = 0f, bool shouldLoop = true)
    {
        if (musicSource.clip == musicClip && musicSource.isPlaying) return;

        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.loop = shouldLoop;

        if (fadeDuration > 0f)
        {
            StartCoroutine(FadeInMusic(fadeDuration));
        }
        else
        {
            musicSource.Play();
        }

        lastPlayedClip = musicClip;
    }
    public void StopMusic(float fadeDuration = 0f)
    {
        if (fadeDuration > 0f)
        {
            StartCoroutine(FadeOutMusic(fadeDuration));
        }
        else
        {
            musicSource.Stop();
        }
    }

    private IEnumerator FadeInMusic(float duration)
    {
        float targetVolume = musicSource.volume;
        musicSource.volume = 0;
        musicSource.Play();

        float timer = 0;
        while (timer < duration)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;

        float timer = 0;
        while (timer < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; 
    }
    public void SetSceneMusic(AudioClip newClip, float fadeDuration = 0.5f, bool shouldLoop = true)
    {
        if (newClip == null || newClip == lastPlayedClip) return;

        PlayMusic(newClip, musicSource.volume, fadeDuration, shouldLoop);
    }

}
