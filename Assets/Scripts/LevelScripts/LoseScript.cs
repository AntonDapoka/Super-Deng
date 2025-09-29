using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        DisableEverything();

        LSDS.StartFullShutDown();
        
        audioSourceGameOver.clip = sound;
        audioSourceGameOver.Play();

        StartCoroutine(FadeOutCoroutine());
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
