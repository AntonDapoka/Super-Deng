using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelLaunchScript : MonoBehaviour
{
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;

    private void Start()
    {
        musicManager.clip = musicTrack;

        TC.StartTimerController(musicTrack.length); //Запускаем таймер на продолжительность трека

        musicManager.Play();

        SCD.StartStartCountDown(); //Запускаем анимацию отсчета
    }
}
