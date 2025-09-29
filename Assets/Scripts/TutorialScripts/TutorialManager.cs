using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;

    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private TextMeshPro tutorialText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject faceStart;
    [SerializeField] private GameObject faceDestination;
    [SerializeField] private TutorialSettings tutorialSettings;
    [SerializeField] private float defaultTypeSpeed = 0.1f;
    [SerializeField] private float fastTypeSpeed = 0.05f;
    [SerializeField] private BeatController beatController;
    [SerializeField] private RedFaceScript redFaceScript;
    [SerializeField] private float fadeInDuration = 2f;

    private float currentTypeSpeed;
    [SerializeField] private int currentMessageIndex = -1;
    private bool isTyping;
    //private bool isWaiting;
    private Coroutine typingCoroutine;

    private void Start()
    {
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        currentTypeSpeed = defaultTypeSpeed;
        isTyping = false;
        tutorialText.text = "";
        faceDestination.SetActive(false);

        musicManager.clip = musicTrack;
        musicManager.volume = 0f;
        musicManager.Play();

        LoadKeyBindings();
        //DisableAllFaces();
        DisableRenderers(player);

        StartCoroutine(FadeInAudio(musicManager, fadeInDuration));
        StartCoroutine(StartTutorialSequence());
    }

    private void LoadKeyBindings()
    {
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));
    }
    /*
    private void DisableAllFaces()
    {
        foreach (var face in FindObjectsOfType<TutorialFaceScript>())
        {
            DisableRenderers(face.gameObject);
        }
    }*/

    private void Update()
    {
        HandleInput();

        if (faceDestination != null && Vector3.Distance(player.transform.position, faceDestination.transform.position) < 0.1f)
        {
            Destroy(faceDestination);
            currentMessageIndex++;
            DisplayMessage(currentMessageIndex);
        }
    }

    private void HandleInput()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") 
            || Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
        {
            if (!IsValidKeyPress())
            {
                if (isTyping)
                {
                    currentTypeSpeed = fastTypeSpeed;
                }
                else if (currentMessageIndex < 4)
                {
                    StartCoroutine(StartTutorialSequence());
                    //DisplayMessage(currentMessageIndex);
                }
            }
            else if (Input.GetKeyDown(keyTop) && currentMessageIndex == 4)
            {
                //UnlockTopFaces();
                currentMessageIndex++;
                DisplayMessage(currentMessageIndex);
            }
            else if (IsValidKeyPress() && currentMessageIndex == 5)
            {
                currentMessageIndex++;
                DisplayMessage(currentMessageIndex);
                faceDestination.SetActive(true);
            }
        }
    }

    private bool IsValidKeyPress()
    {
        return Input.GetKeyDown(keyLeft) || Input.GetKeyDown(keyTop) || Input.GetKeyDown(keyRight);
    }
    /*
    private void UnlockTopFaces()
    {
        foreach (var face in FindObjectsOfType<TutorialFaceScript>())
        {
            if (face.isTop)
            {
                EnableRenderers(face.gameObject);
                face.isBlocked = false;
            }
        }
    }*/

    private void DisplayMessage(int messageIndex)
    {
        if (messageIndex < tutorialSettings.CountMessages)
        {
            currentMessageIndex = messageIndex;
            StartCoroutine(TypeText(tutorialSettings[messageIndex].Message));
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator StartTutorialSequence()
    {
        currentMessageIndex++;
        //isWaiting = true;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        DisplayMessage(currentMessageIndex);

        if (currentMessageIndex == 2)
        {
            EnableRenderers(player);
            EnableRenderers(faceStart);
        }
        else if (currentMessageIndex == 4)
        {
            //UnlockTopFaces();
        }

        yield return new WaitForSeconds(rhythmManager.beatInterval * 3);
        //isWaiting = false;

        if (currentMessageIndex < 4)
        {
            typingCoroutine = StartCoroutine(StartTutorialSequence());
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        tutorialText.text = "";

        foreach (char letter in text)
        {
            tutorialText.text += letter;
            yield return new WaitForSeconds(currentTypeSpeed);
        }

        isTyping = false;
        currentTypeSpeed = defaultTypeSpeed;
    }

    private void EnableRenderers(GameObject target)
    {
        if (target != null)
        {
            foreach (var renderer in target.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
        }
    }

    private void DisableRenderers(GameObject target)
    {
        if (target != null)
        {
            foreach (var renderer in target.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }
    }

    private IEnumerator FadeInAudio(AudioSource audioSource, float duration)
    {
        float elapsedTime = 0f;
        float startVolume = 0f;
        float targetVolume = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}