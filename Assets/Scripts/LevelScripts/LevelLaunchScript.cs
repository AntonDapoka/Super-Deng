using UnityEngine;

public class LevelLaunchScript : MonoBehaviour
{
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RhythmManager RM;
    

    private void Start()
    {
        RM.SetAudioSource(musicManager);
        musicManager.clip = musicTrack;
        musicManager.Play();
        TC.StartTimerController(musicTrack.length); //Запускаем таймер на продолжительность трека
        SCD.StartStartCountDown(); //Запускаем анимацию отсчета
        
    }
}
