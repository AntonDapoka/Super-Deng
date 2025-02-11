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
        musicManager.Play();
        TC.StartTimerController(musicTrack.length); //��������� ������ �� ����������������� �����
        SCD.StartStartCountDown(); //��������� �������� �������
        
    }
}
