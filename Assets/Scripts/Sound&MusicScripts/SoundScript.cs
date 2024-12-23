using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public AudioClip soundClipStep;
    public AudioClip soundClipBlock;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.clip = soundClipStep;
    }

    public void TurnOnSoundStep()
    {
        audioSource.clip = soundClipStep;
        if (audioSource != null && soundClipStep != null)
        {
            audioSource.Play();
        }
    }

    public void TurnOnSoundBlock()
    {
        audioSource.clip = soundClipBlock;
        if (audioSource != null && soundClipBlock != null)
        {
            audioSource.Play();
        }
    }
}
