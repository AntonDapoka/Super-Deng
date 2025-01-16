using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatFlickeringScript : MonoBehaviour
{
    public bool isTurnOn = false;
    [SerializeField] private RhythmManager RM;
    private float beatInterval;
    private float elapsedTime = 0f;
    public bool canPress = false;
    public bool canCombo = false;


    public Material materialToFade;

    public int beatcount = 0;
    private float lastBeatTime = 0f;
    public bool isAlreadyPressed = false;
    public bool isAlreadyPressedIsAlreadyPressed = false;

    private void Start()
    {
        beatInterval = RM.beatInterval;

        SetMaterialAlpha(0f);
    }

    private void Update()
    {
        if (isTurnOn)
        {
            if (0f < elapsedTime && ((elapsedTime < 0.25f * beatInterval) || (elapsedTime > 0.75f * beatInterval)))
            {
                canCombo = true;
            }
            else canCombo = false;

            if (elapsedTime > 0f)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime < beatInterval / 3f)
                {

                    if (!isAlreadyPressed)
                        canPress = true;
                    else
                        canPress = false;
                    float t = elapsedTime / (beatInterval / 3f);

                    //float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                    //SetMaterialAlpha(newAlpha);

                }
                else if (elapsedTime < (2f * beatInterval) / 3f)
                {
                    canPress = false;


                    if (!isAlreadyPressedIsAlreadyPressed)
                        PressIsAlreadyPress();

                    float t = (elapsedTime - (beatInterval / 3f)) / (beatInterval / 3f);
                    //image1.enabled = true;
                    //image1.rectTransform.localPosition = Vector3.Lerp(startPos1, midPos1, t);
                    //image2.enabled = true;
                    //image2.rectTransform.localPosition = Vector3.Lerp(startPos2, midPos2, t);
                }
                else if (elapsedTime < beatInterval)
                {
                    if (!isAlreadyPressed)
                        canPress = true;
                    else
                        canPress = false;
                    float t = (elapsedTime - ((2f * beatInterval) / 3f)) / (beatInterval / 3f);
                    //image1.rectTransform.localPosition = Vector3.Lerp(midPos1, almostEndPos1, t);
                    //image2.rectTransform.localPosition = Vector3.Lerp(midPos2, almostEndPos2, t);
                }
                else
                {
                    elapsedTime = 0f;
                }
            }
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
        //image.rectTransform.localPosition = startPos; // “елепортирование на начальную позицию
    }
    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        Color color = materialToFade.color;
        float startAlpha = color.a;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            SetMaterialAlpha(newAlpha);
            yield return null;
        }

        // ”бедимс€, что финальное значение точно установлено
        SetMaterialAlpha(targetAlpha);
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color color = materialToFade.color;
        color.a = alpha;
        materialToFade.color = color;
    }
}
