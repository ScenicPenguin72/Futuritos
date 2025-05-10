
using UnityEngine;
using UnityEngine.Video;
public class PlayVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public void PlayVideoUI()
    {
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += vp => vp.Play();
    }
}

