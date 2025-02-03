using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public int index = 0;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[index];
        audioClips[index+1].LoadAudioData();
        audioSource.Play();
    }
    public void RotateSong()
    {
        index = (index + 1) % audioClips.Length;

        audioSource.clip = audioClips[index];
        audioSource.Play();
        if (!audioClips[(index + 1) % audioClips.Length].loadState.Equals(AudioDataLoadState.Loaded))
        {
            audioClips[(index + 1) % audioClips.Length].LoadAudioData();
        }
    }


}