using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityTauntInteractorScript : MonoBehaviour, IAbilityScript
{
    public AudioClip soundClipTaunt;
    public AudioSource audioSource;

    public void Activate(GameObject face)
    {
        audioSource.clip = soundClipTaunt;
        if (audioSource != null && soundClipTaunt != null)
        {
            audioSource.Play();
        }
    }

}
