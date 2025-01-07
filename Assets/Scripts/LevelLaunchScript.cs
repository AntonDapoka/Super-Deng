using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelLaunchScript : MonoBehaviour
{
    [SerializeField] AudioSource musicManager;
    [SerializeField] AudioClip musicTrack;
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;

    private void Start()
    {
        musicManager.clip = musicTrack;
        TC.StartTimerController(musicTrack.length);
        musicManager.Play();
        SCD.StartStartCountDown();
    }
}
