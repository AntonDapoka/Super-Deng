using UnityEngine;

public class RedWaveScript
{
    private int id;
    private float timer; // Local
    private bool isTime = true;
    private bool isBroken = false;
    private bool hasBudded = false;
    private readonly GameObject face;
    private readonly FaceScript faceScript;
    private readonly FaceStateScript faceState;
    private readonly RedWaveSpawnerPresenterScript presenter;

    private State state;

    private enum State
    {
        Coloring,
        ScaleUp,
        Wait,
        ScaleDown,
        Done
    }

    private readonly float colorDuration;
    private readonly float scaleUpDuration;
    private readonly float waitDuration;
    private readonly float scaleDownDuration;
    private readonly float height;
    private readonly float offset;
    private readonly Material materialAction;
    private readonly bool isLifeDurationEnabled;
    private float lifeDuration;
    private readonly RedWaveBuddingType buddingType;

    private readonly Vector3 startScale;
    private readonly Vector3 targetScale;
    private readonly Vector3 startPos;
    private readonly Vector3 targetPos;

    public bool IsFinished => state == State.Done;
    public bool ShouldBud { get; private set; }
    public GameObject CurrentFace => face;

    public RedWaveScript(
        GameObject face,
        RedWaveSettings settings,
        RedWaveBasicSettings settingsBasic,
        RedWaveSpawnerPresenterScript presenter)
    {
        this.face = face;
        this.presenter = presenter;

        buddingType = settings != null ? settings.typeRedWaveBudding : RedWaveBuddingType.isBuddingAfterColoring;
        isLifeDurationEnabled = settings != null && settings.isLifeDuration;
        lifeDuration = settings != null ? settings.lifeDurationSeconds : 0f;

        bool isChange = settings != null && settings.isBasicSettingsChange;

        if (settings != null && isChange && settings.isColorDurationChange)
            colorDuration = settings.colorDurationSeconds;
        else colorDuration = settingsBasic.colorDurationSecondsBasic;

        if (settings != null && isChange && settings.isScaleUpDurationChange)
            scaleUpDuration = settings.scaleUpDurationSeconds;
        else scaleUpDuration = settingsBasic.scaleUpDurationSecondsBasic;

        if (settings != null && isChange && settings.isWaitDurationChange)
            waitDuration = settings.waitDurationSeconds;
        else waitDuration = settingsBasic.waitDurationSecondsBasic;

        if (settings != null && isChange && settings.isScaleDownDurationChange)
            scaleDownDuration = settings.scaleDownDurationSeconds;
        else scaleDownDuration = settingsBasic.scaleDownDurationSecondsBasic;

        if (settings != null && isChange && settings.isHeightChange)
            height = settings.height;
        else height = settingsBasic.heightBasic;

        if (settings != null && isChange && settings.isOffsetChange)
            offset = settings.offset;
        else offset = settingsBasic.offsetBasic;

        if (settings != null && isChange && settings.isMaterialChange)
            materialAction = settings.material;
        else materialAction = settingsBasic.materialBasic;

        faceScript = face.GetComponent<FaceScript>();
        faceState = face.GetComponent<FaceStateScript>();

        startScale = faceScript.glowingPart.transform.localScale;
        startPos = faceScript.glowingPart.transform.localPosition;

        targetScale = new Vector3(1f, 1f, height);
        targetPos = new Vector3(0f, offset, 0f);

        ShouldBud = false;
        StartColoring();
    }

    public void Update()
    {
        if (state == State.Done || isBroken) return;

        if (isLifeDurationEnabled)
        {
            lifeDuration -= Time.deltaTime;
            if (lifeDuration <= 0f)
            {
                ForcedBreak();
                return;
            }
        }

        switch (state)
        {
            case State.Coloring: UpdateColoring(); break;
            case State.ScaleUp: UpdateScaleUp(); break;
            case State.Wait: UpdateWait(); break;
            case State.ScaleDown: UpdateScaleDown(); break;
        }
    }

    private void StartColoring()
    {
        if (state == State.Done || isBroken) return;

        timer = 0f;
        state = State.Coloring;
        faceState.SetFaceState(FaceProperty.IsColored, true);
        ApplyRedWaveMaterial();
    }

    private void UpdateColoring()
    {
        AdvanceTimer();
        if (TimerExpired(colorDuration))
        {
            CheckAndTriggerBudding(State.Coloring);
            StartScaleUp();
        }
    }

    private void StartScaleUp()
    {
        timer = 0f;
        state = State.ScaleUp;

        faceState.SetFaceState(FaceProperty.IsKilling, true);
        faceState.SetFaceState(FaceProperty.IsColored, false);

        ApplyRedWaveMaterial();
    }

    private void UpdateScaleUp()
    {
        UpdateScaling(Vector3.zero, targetScale, startPos, targetPos, scaleUpDuration, StartWait);
    }

    private void StartWait()
    {
        CheckAndTriggerBudding(State.ScaleUp);
        timer = 0f;
        state = State.Wait;
        ApplyRedWaveMaterial();
    }

    private void UpdateWait()
    {
        AdvanceTimer();

        if (TimerExpired(waitDuration))
            StartScaleDown();
    }

    private void StartScaleDown()
    {
        CheckAndTriggerBudding(State.Wait);
        timer = 0f;
        state = State.ScaleDown;
        ApplyRedWaveMaterial();
    }

    private void UpdateScaleDown()
    {
        UpdateScaling(targetScale, startScale, targetPos, startPos, scaleDownDuration, Finish);
    }

    private void Finish()
    {
        CheckAndTriggerBudding(State.ScaleDown);

        faceScript.glowingPart.transform.localPosition = startPos;
        faceScript.glowingPart.transform.localScale = startScale;

        faceState.SetFaceState(FaceProperty.IsKilling, false);
        presenter.ChangeFaceBackToDefault(face);
        state = State.Done;
    }

    private void UpdateScaling(Vector3 fromScale, Vector3 toScale, Vector3 fromPos, Vector3 toPos, float duration, System.Action onComplete)
    {
        AdvanceTimer();

        float t = Mathf.Clamp01(timer / duration);

        faceScript.glowingPart.transform.localScale = Vector3.Lerp(fromScale, toScale, t);
        faceScript.glowingPart.transform.localPosition = Vector3.Lerp(fromPos, toPos, t);

        if (t >= 1f) onComplete();
    }

    private void ApplyRedWaveMaterial()
    {
        presenter.ApplyFaceActionMaterial(face, materialAction);
    }

    private void AdvanceTimer()
    {
        if (isTime) timer += Time.deltaTime;
    }

    private bool TimerExpired(float duration)
    {
        return timer >= duration;
    }

    private void CheckAndTriggerBudding(State currentState)
    {
        if (hasBudded) return;

        bool shouldTrigger = buddingType switch
        {
            RedWaveBuddingType.isBuddingAfterColoring => currentState == State.Coloring,
            RedWaveBuddingType.isBuddingAfterScalingUp => currentState == State.ScaleUp,
            RedWaveBuddingType.isBuddingAfterWaiting => currentState == State.Wait,
            RedWaveBuddingType.isBuddingAfterScalingDown => currentState == State.ScaleDown,
            _ => false
        };

        if (shouldTrigger)
        {
            ShouldBud = true;
        }
    }

    public void MarkBudded()
    {
        hasBudded = true;
        ShouldBud = false;
    }

    public void ForcedBreak()
    {
        if (isBroken || state == State.Done) return;

        isBroken = true;

        faceScript.glowingPart.transform.localPosition = startPos;
        faceScript.glowingPart.transform.localScale = startScale;

        faceState.SetFaceState(FaceProperty.IsKilling, false);
        faceState.SetFaceState(FaceProperty.IsColored, false);

        presenter.ChangeFaceBackToDefault(face);

        state = State.Done;
    }
}
