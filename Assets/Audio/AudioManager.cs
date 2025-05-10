using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource soundEffectsSource;   // For sound effects
    public AudioSource musicSource;          // For background music

    [Header("Music Clips")]
    public AudioClip menuMusicClip;          // Music clip for the menu scene
    public AudioClip gameMusicClip;          // Music clip for the game scene

    private const string AudioVolumePref = "AudioVolume";
    private const string MusicVolumePref = "MusicVolume";
    private AudioClip lastPlayedClip;  // To track the last played music clip

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set volumes from PlayerPrefs
        float savedAudioVolume = PlayerPrefs.GetFloat(AudioVolumePref, 0.5f); // Default to 1 if not set
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumePref, 0.5f); // Default to 1 if not set

        soundEffectsSource.volume = savedAudioVolume;
        musicSource.volume = savedMusicVolume;

        // Play the appropriate music based on the player's presence in the scene
        PlayMusicBasedOnPlayerPresence();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center
        Cursor.visible = false;                   // Hides the cursor
    }
    private void PlayMusicBasedOnPlayerPresence()
    {
        // Check if the player is in the scene
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            // Play game music if player is present
            PlayMusic(gameMusicClip, musicSource.volume);
        }
        else
        {
            // Play menu music if no player is found
            PlayMusic(menuMusicClip, musicSource.volume);
        }
    }

    // Play a sound effect
    public void PlaySoundEffect(AudioClip clip, float volume = 1f)
    {
        soundEffectsSource.PlayOneShot(clip, volume);
    }

    // Play music track with an optional fade-in
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


    // Stop the music with an optional fade-out
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

    // Coroutine to fade in music
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

    // Coroutine to fade out music
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
        musicSource.volume = startVolume; // Reset volume for next play
    }
    public void SetSceneMusic(AudioClip newClip, float fadeDuration = 0.5f, bool shouldLoop = true)
    {
        if (newClip == null || newClip == lastPlayedClip) return;

        PlayMusic(newClip, musicSource.volume, fadeDuration, shouldLoop);
    }

}
