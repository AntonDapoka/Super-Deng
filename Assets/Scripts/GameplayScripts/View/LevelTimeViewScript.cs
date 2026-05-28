using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimeViewScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider timerSlider;

    public void InitializeTime(string timeStartFormattedString, float timeStart, float timeTotal)
    {
        timerText.text = timeStartFormattedString;
        timerSlider.maxValue = timeTotal;
        timerSlider.value = timeStart;
    }

    public void DisplayTime(string timeElapsedFormattedString, float timeElapsed)
    {
        timerText.text = timeElapsedFormattedString;
        UpdateSliderDisplay(timeElapsed);
    }

    private void UpdateSliderDisplay(float time)
    {
        timerSlider.value = time;
    }
}
