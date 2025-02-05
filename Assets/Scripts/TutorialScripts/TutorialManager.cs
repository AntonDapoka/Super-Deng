using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private TextMeshPro textMain;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject faceStart;
    public TutorialSettings _tutorialSettings;
    [SerializeField] private float _timeTypeSymbolDefault;
    [SerializeField] private float _timeTypeSymbolSpeedUp;
    [SerializeField] private BeatController BC;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private float fadeInDuration = 2f;
    private float _timeTypeSymbolCurrent;
    public int index = -1;
    private bool isWriting;
    private bool isWaiting;
    private Coroutine currentCoroutine;

    private void Start()
    {
        _timeTypeSymbolCurrent = _timeTypeSymbolDefault;
        isWriting = false;
        textMain.text = "";

        musicManager.clip = musicTrack;
        musicManager.volume = 0f; // Начинаем с нулевой громкости
        musicManager.Play();

        GameObject[] faces = FindObjectsOfType<TutorialFaceScript>()
            .Select(faceScript => faceScript.gameObject)
            .ToArray();

        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i] != null)
            {
                DisableRenderers(faces[i]);
            }
        }
        DisableRenderers(player);

        StartCoroutine(FadeIn(musicManager, fadeInDuration));

        StartCoroutine(AfterLoadWaiting());
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) ||
            Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") ||
            Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
        {
            if (isWriting) _timeTypeSymbolCurrent = _timeTypeSymbolSpeedUp;
            else if (!isWaiting)
            {
                StartCoroutine(SetAccess());
            }
        }
    }

    private IEnumerator AfterLoadWaiting()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(SetAccess());
    }

    private void SetMessage(int newIndex)
    {
        if (newIndex < _tutorialSettings.CountMessages)
        {
            index = newIndex;
            StartCoroutine(TypingText(_tutorialSettings[index].Message));
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator SetAccess()
    {
        index++;
        isWaiting = true;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        SetMessage(index);

        GameObject[] faces = FindObjectsOfType<TutorialFaceScript>()
            .Select(faceScript => faceScript.gameObject)
            .ToArray();
        
        if (index == 1)
        {

            EnableRenderers(player);
            EnableRenderers(faceStart);

            yield return new WaitForSeconds(2f);
        }
        else if (index == 2)
        {
            for (int i = 0; i < faces.Length; i++)
            {

                if (faces[i].GetComponent<TutorialFaceScript>().isTop)
                {
                    EnableRenderers(faces[i]);
                    Debug.Log(faces[i].name);
                }
            }
            yield return new WaitForSeconds(2f);
        }
        else if (index == 3)
        {
            for (int i = 0; i < faces.Length; i++)
            {
                if (faces[i] != null)
                {
                    Debug.Log("aaa");
                    EnableRenderers(faces[i]);
                }
            }
            yield return new WaitForSeconds(2f);
        }
        isWaiting = false;
        if (index < 4) currentCoroutine = StartCoroutine(SetAccess());
    }

    public void LoseTutorial()
    {
        TypingText("Loser");
    }


    private IEnumerator TypingText(string text)
    {
        isWriting = true;
        textMain.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            textMain.text += text[i];
            yield return new WaitForSeconds(_timeTypeSymbolCurrent);
        }
        isWriting = false;
        _timeTypeSymbolCurrent = _timeTypeSymbolDefault;
        yield return null;
    }

    private void EnableRenderers(GameObject side)
    {
        Renderer[] childRenderers = side.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = true;
        }
    }

    private void DisableRenderers(GameObject side)
    {
        
        Renderer[] childRenderers = side.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = false;
        }
        //side.SetActive(false);
    }

    IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float startVolume = 0f;
        float targetVolume = 1f; // Максимальная громкость

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume; // Убедимся, что громкость установлена в 1 после завершения
    }
}
