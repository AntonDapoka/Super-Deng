using UnityEngine;

public class BeatController : MonoBehaviour //Скрипт под снос
{
    public bool isTurnOn = false;
    public bool isTutorial = false;
    [SerializeField] private RhythmManager RM;
    private float beatInterval;
    private float elapsedTime = 0f;
    public bool canPress = false;
    public bool canCombo = false;


    public int beatcount = 0;
    private float lastBeatTime = 0f;
    public bool isAlreadyPressed = false;
    public bool isAlreadyPressedIsAlreadyPressed = false;

    private void Start()
    {
        beatInterval = RM.beatInterval;
    }

    private void Update()
    {
        if (!isTurnOn) return;

        canCombo = (elapsedTime > 0f) && ((elapsedTime < 0.25f * beatInterval) || (elapsedTime > 0.75f * beatInterval));

        if (elapsedTime <= 0f) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime < beatInterval / 3f)
        {
            canPress = !isAlreadyPressed;
        }
        else if (elapsedTime < (2f * beatInterval) / 3f)
        {
            canPress = false;
            if (!isAlreadyPressedIsAlreadyPressed)
                PressIsAlreadyPress();
        }
        else if (elapsedTime < beatInterval)
        {
            canPress = !isAlreadyPressed;
        }
        else
        {
            elapsedTime = 0f;
        }
    }

    private void PressIsAlreadyPress()
    {
        isAlreadyPressed = false;
        isAlreadyPressedIsAlreadyPressed = true;
    }

    public void OnBeat()
    {
        float currentTime = Time.time;
        if (lastBeatTime != 0f)
        {
            beatInterval = currentTime - lastBeatTime;
        }
        lastBeatTime = currentTime;
        beatcount++;
        elapsedTime = 0.0001f; 
        //image.rectTransform.localPosition = startPos; // Телепортирование на начальную позицию
    }
}