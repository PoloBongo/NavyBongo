using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] playlist;
    private int currentTrackIndex = 0;

    void Start()
    {
        if (playlist.Length > 0)
        {
            PlayTrack(currentTrackIndex);
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            NextTrack();
        }
    }

    void PlayTrack(int index)
    {
        if (index >= 0 && index < playlist.Length)
        {
            audioSource.clip = playlist[index];
            audioSource.Play();
        }
    }

    void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
        PlayTrack(currentTrackIndex);
    }
}