using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseScript : MonoBehaviour
{
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private LightShutDownScript LSDS;
    [SerializeField] private IcoSphereDanceScript ISDS;
    [SerializeField] private AudioClip sound;
    [SerializeField] private Image imageLose;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceGameOver;
    [SerializeField] private float fadeDuration = 2f;

    public void Lose()
    {
        RFS.isTurnOn = false;
        ISDS.isTurnOn = false;
        if (TC != null)
        {
            TC.isTurnOn = false;
        }
        if (LSDS != null)
        {
            LSDS.StartShutDown();
        }
        else ShowImage();
        audioSourceGameOver.clip = sound;
        audioSourceGameOver.Play();

        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {

        float startVolume = audioSourceMusic.volume;

        while (audioSourceMusic.volume > 0)
        {
            audioSourceMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSourceMusic.volume = 0;
        audioSourceMusic.Stop();
    }

    public void ShowImage()
    {
        imageLose.gameObject.SetActive(true);
    }

}
