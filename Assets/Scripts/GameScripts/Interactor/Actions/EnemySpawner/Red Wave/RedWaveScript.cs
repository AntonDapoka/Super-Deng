using UnityEngine;

public class RedWaveScript
{
    private float timer; //Local
    private bool isTime = true;
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

    private readonly Material material; //MOVE TO PRESENTER

    private readonly Vector3 startScale;
    private readonly Vector3 targetScale;
    private readonly Vector3 startPos;
    private readonly Vector3 targetPos;

    public bool IsFinished => state == State.Done;

    public RedWaveScript(
        GameObject face,
        RedWaveSettings settings,
        RedWaveBasicSettings settingsBasic,
        RedWaveSpawnerPresenterScript presenter)
    {
        this.face = face;
        this.presenter = presenter;

        if (settings.isColorDurationChange)
            colorDuration = settings.colorDurationSeconds;
        else colorDuration = settingsBasic.colorDurationSecondsBasic;

        if (settings.isScaleUpDurationChange)
            scaleUpDuration = settings.scaleUpDurationSeconds;
        else scaleUpDuration = settingsBasic.scaleUpDurationSecondsBasic;

        if (settings.isWaitDurationChange)
            waitDuration = settings.waitDurationSeconds;
        else waitDuration = settingsBasic.waitDurationSecondsBasic;

        if (settings.isScaleDownDurationChange)
            scaleDownDuration = settings.scaleDownDurationSeconds;
        else scaleDownDuration = settingsBasic.scaleDownDurationSecondsBasic;

        if (settings.isHeightChange)
            height = settings.height;
        else height = settingsBasic.heightBasic;

        if (settings.isOffsetChange)
            offset = settings.offset;
        else offset = settingsBasic.offsetBasic;

        if (settings.isMaterialChange)
            material = settings.material;
        else material = settingsBasic.materialBasic;

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
        timer = 0f;
        state = State.Coloring;
        faceState.SetFaceState(FaceProperty.IsColored, true);
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

        faceState.SetFaceState(FaceProperty.IsKilling, true);
        faceState.SetFaceState(FaceProperty.IsColored, false);
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

        if (TimerExpired(waitDuration))
            StartScaleDown();
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
        faceScript.rend.material = material; //CHANGE TO PRESENTER
        faceState.SetFaceState(FaceProperty.IsKilling, false);
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
        faceScript.rend.material = material;
        //CHANGE TO PRESENTER
    }

    private void AdvanceTimer()
    {
        if (isTime)
            timer += Time.deltaTime;
    }

    private bool TimerExpired(float duration)
    {
        return timer >= duration;
    }

    public void ForcedBreak()
    {
        state = State.Done;
        faceState.SetFaceState(FaceProperty.IsKilling, false);
        faceState.SetFaceState(FaceProperty.IsColored, false);
        //faceScript.glowingPart.transform.localPosition = 
        //faceScript.glowingPart.transform.localScale = 
        //presenter.SetBasicFace();
    }
}