using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScript : MonoBehaviour
{
    [SerializeField] private Image imageCompleted;
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

    public void Win()
    {
        ShowImage();

        DisableEverything();
    }

    private void ShowImage()
    {
        imageCompleted.gameObject.SetActive(true);
    }

    private void DisableEverything()
    {
        TC.isTurnOn = false;
        UFMS.isTurnOn = false;
        RM.isTurnOn = false;
        RFS.isTurnOn = false;
        RWS.isTurnOn = false;
        FM.isTurnOn = false;
        BSS.isTurnOn = false;
        PSS.isTurnOn = false;
        CZ.isTurnOn = false;
        PTTB.isTurnOn = false;
        ISDS.isTurnOn = false;
    }
}
