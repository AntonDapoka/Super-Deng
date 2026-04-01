using UnityEngine;

public class RedFaceScript 
{
    private int id;
    private float timer; //Local
    private bool isTime = true;
    private readonly GameObject face;
    private readonly FaceScript faceScript;
    private readonly FaceStateScript faceState;
    private readonly RedFaceSpawnerPresenterScript presenter;

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
    private readonly Material materialAction;
    private readonly float offset;
    private readonly float height;

    private readonly Vector3 startScale;
    private readonly Vector3 targetScale;
    private readonly Vector3 startPos;
    private readonly Vector3 targetPos;

    public bool IsFinished => state == State.Done;
    private bool isBroken;

    public RedFaceScript(
        GameObject face,
        RedFaceSettings settings,
        RedFaceBasicSettings settingsBasic,
        RedFaceSpawnerPresenterScript presenter)
    {
        this.face = face;
        this.presenter = presenter;

        bool isChange = settings.isBasicSettingsChange;

        if (isChange && settings.isColorDurationChange)
            colorDuration = settings.colorDurationSeconds;
        else colorDuration = settingsBasic.colorDurationSecondsBasic;

        if (isChange && settings.isScaleUpDurationChange)
            scaleUpDuration = settings.scaleUpDurationSeconds;
        else scaleUpDuration = settingsBasic.scaleUpDurationSecondsBasic;

        if (isChange && settings.isWaitDurationChange)
            waitDuration = settings.waitDurationSeconds;
        else waitDuration = settingsBasic.waitDurationSecondsBasic;

        if (isChange && settings.isScaleDownDurationChange)
            scaleDownDuration = settings.scaleDownDurationSeconds;
        else scaleDownDuration = settingsBasic.scaleDownDurationSecondsBasic;

        if (isChange && settings.isHeightChange)
            height = settings.height;
        else height = settingsBasic.heightBasic;

        if (isChange && settings.isOffsetChange)
            offset = settings.offset;
        else offset = settingsBasic.offsetBasic;

        if (isChange && settings.isMaterialChange)
            materialAction = settings.material;
        else materialAction = settingsBasic.materialBasic;

        faceScript = face.GetComponent<FaceScript>();
        faceState = face.GetComponent<FaceStateScript>();

        startScale = faceScript.glowingPart.transform.localScale;
        startPos = faceScript.glowingPart.transform.localPosition;

        targetScale = new Vector3(1f, 1f, height);
        targetPos = new Vector3(0f, offset, 0f);

        StartColoring();
    }


    public void Update()
    {
        if (state == State.Done || isBroken) return;

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
        faceState.Set(FaceProperty.IsColored, true);
    }

    private void UpdateColoring()
    {
        ApplyRedFaceMaterial();
        AdvanceTimer();
        if (TimerExpired(colorDuration)) StartScaleUp();
    }

    private void StartScaleUp()
    {
        timer = 0f;
        state = State.ScaleUp;

        faceState.Set(FaceProperty.IsKilling, true);
        faceState.Set(FaceProperty.IsColored, false);
    }

    private void UpdateScaleUp()
    {
        UpdateScaling(startScale, targetScale, startPos, targetPos, scaleUpDuration, StartWait);
    }

    private void StartWait()
    {
        timer = 0f;
        state = State.Wait;
    }

    private void UpdateWait()
    {
        AdvanceTimer();

        if (TimerExpired(waitDuration)) StartScaleDown();
    }

    private void StartScaleDown()
    {
        timer = 0f;
        state = State.ScaleDown;
    }

    private void UpdateScaleDown()
    {
        UpdateScaling(targetScale, startScale, targetPos, startPos, scaleDownDuration, Finish);
    }

    private void Finish()
    {
        faceScript.glowingPart.transform.localPosition = startPos;
        faceScript.glowingPart.transform.localScale = startScale;
        
        faceState.Set(FaceProperty.IsKilling, false);
        presenter.ChangeFaceBackToDefault(face);
        state = State.Done;
    }

    private void UpdateScaling(Vector3 fromScale, Vector3 toScale, Vector3 fromPos, Vector3 toPos, float duration, System.Action onComplete)
    {
        ApplyRedFaceMaterial();
        AdvanceTimer();

        float t = Mathf.Clamp01(timer / duration);

        faceScript.glowingPart.transform.localScale = Vector3.Lerp(fromScale, toScale, t);
        faceScript.glowingPart.transform.localPosition = Vector3.Lerp(fromPos, toPos, t);

        if (t >= 1f) onComplete();
    }

    private void ApplyRedFaceMaterial()
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

    public void ForcedBreak()
    {
        if (isBroken || state == State.Done) return;

        isBroken = true;

        faceScript.glowingPart.transform.localPosition = startPos;
        faceScript.glowingPart.transform.localScale = startScale;

        faceState.Set(FaceProperty.IsKilling, false);
        faceState.Set(FaceProperty.IsColored, false);

        presenter.ChangeFaceBackToDefault(face);

        state = State.Done;
    }
}