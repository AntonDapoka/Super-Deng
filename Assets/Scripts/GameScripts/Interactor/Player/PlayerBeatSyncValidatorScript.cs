using UnityEngine;

public class PlayerBeatSyncValidatorScript : MonoBehaviour, IBeatUpdate
{
    [SerializeField] private bool isTurnOn = false;

    private float beatInterval;
    private float elapsedTime;

    private bool canPress;
    private bool canCombo;

    private float lastBeatTime;

    private bool hasPressedThisBeat;
    private bool wasResetThisBeat;

    private int beatCount = 0;

    [SerializeField] private float comboWindowEarly = 0.25f;
    [SerializeField] private float comboWindowLate = 0.75f;

    [SerializeField] private float pressWindowEarly = 0.33f;
    [SerializeField] private float pressWindowLate = 0.67f;

    public void Initialize(float beatInterval)
    {
        this.beatInterval = beatInterval;

        isTurnOn = true;
        /*
        comboWindowEarly = xx;
        comboWindowLate = yy;
        pressWindowEarly = xx;
        pressWindowLate = yy;
        */
    }

    private void Update()
    {
        if (!isTurnOn || elapsedTime <= 0f) return;

        elapsedTime += Time.deltaTime;

        UpdateComboWindow();
        UpdatePressWindow();

        if (elapsedTime >= beatInterval)
            ResetBeatCycle();
    }

    private void UpdateComboWindow()
    {
        float normalizedTime = elapsedTime / beatInterval;

        canCombo = normalizedTime < comboWindowEarly || normalizedTime > comboWindowLate;
    }

    private void UpdatePressWindow()
    {
        if (elapsedTime < pressWindowEarly * beatInterval)
        {
            canPress = !hasPressedThisBeat;
        }
        else if (elapsedTime < pressWindowLate * beatInterval)
        {
            canPress = false;

            if (!wasResetThisBeat)
                ResetPressStateMidBeat();
        }
        else
        {
            canPress = !hasPressedThisBeat;
        }
    }

    private void ResetPressStateMidBeat()
    {
        hasPressedThisBeat = false;
        wasResetThisBeat = true;
    }

    private void ResetBeatCycle()
    {
        elapsedTime = 0f;
        wasResetThisBeat = false;
    }

    public void OnBeat()
    {
        float currentTime = Time.time;

        if (lastBeatTime > 0f)
            beatInterval = currentTime - lastBeatTime;

        lastBeatTime = currentTime;
        beatCount++;
        elapsedTime = 0.0001f;
        hasPressedThisBeat = false;
        wasResetThisBeat = false;
    }

    public void RegisterPress()
    {
        if (hasPressedThisBeat) return;

        hasPressedThisBeat = true;
        wasResetThisBeat = true;
    }

    public bool CanPress() => canPress;
    public bool CanCombo() => canCombo;
}