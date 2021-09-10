using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectsPlayer : MonoBehaviour
{
    public AudioClip[] audioClips;

    public AudioSource audioPlayer;

    public delegate void PlayAudio(int index);
    public static PlayAudio PlayAudioClip;

    private void Start()
    {
        PlayAudioClip += PlayClip;
    }

    private void OnDisable()
    {
        PlayAudioClip -= PlayClip;
    }
    public void PlayClip(int clipIndex)
    {
        audioPlayer.PlayOneShot(audioClips[clipIndex]);
    }
}
