using UnityEngine;

public class MusicaNivel : MonoBehaviour
{
    public AudioClip level2Music;
    public bool shouldLoop = true;  

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSceneMusic(level2Music, 0.5f, shouldLoop);
        }
    }
}
