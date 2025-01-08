using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseScript : MonoBehaviour
{
    [SerializeField] private UnifiedFrameManagerScript UFMS;
    [SerializeField] private RhythmManager RM;
    [SerializeField] private TimerController TC;
    [SerializeField] private StartCountDown SCD;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private RedWaveScript RWS;
    [SerializeField] private FallManager FM;
    [SerializeField] private BonusSpawnerScript BSS;
    [SerializeField] private PortalSpawnerScript PSS;
    [SerializeField] private LightShutDownScript LSDS;
    [SerializeField] private IcoSphereDanceScript ISDS;
    [SerializeField] private AudioClip sound;
    [SerializeField] private Image imageLose;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceGameOver;
    [SerializeField] private CameraZoom CZ;
    [SerializeField] private PulseToTheBeat PTTB;

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

    private void DisableEverything()
    {
        UFMS.isTurnOn = true;
        RM.StartWithSync();
        RFS.isTurnOn = true;
        RWS.isTurnOn = true;
        FM.isTurnOn = true;
        BSS.isTurnOn = true;
        PSS.isTurnOn = true;
        CZ.isTurnOn = true;
        PTTB.isTurnOn = true;
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
