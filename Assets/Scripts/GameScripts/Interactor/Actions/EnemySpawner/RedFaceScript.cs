using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFaceScript 
{
    private bool isTime = true;
    private readonly GameObject face;
    private readonly FaceScript faceScript;
    private readonly FaceStateScript faceState;

    private readonly RedFaceSettings settings;
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

    private float timer; //Local

    private readonly bool isBasicSettingsChange;

    private readonly bool isMaterialChange;
    private readonly Material material;

    private readonly bool isColorDurationChange;
    private readonly float colorDuration; //

    private readonly bool isScaleUpDurationChange;
    private readonly float scaleUpDuration;//

    private readonly bool isWaitDurationChange;
    private readonly float waitDuration;//

    private readonly bool isScaleDownDurationChange;
    private readonly float scaleDownDuration;//

    private readonly bool isHeightChange;
    private readonly float height;

    private readonly bool isOffsetChange;
    private readonly float offset;

    private readonly Vector3 startScale;
    private readonly Vector3 targetScale;
    private readonly Vector3 startPos;
    private readonly Vector3 targetPos;

    public bool IsFinished => state == State.Done;

    public RedFaceScript(
        GameObject face,
        RedFaceSettings settings,
        RedFaceSpawnerPresenterScript presenter)
    {
        this.face = face;
        this.settings = settings;
        this.presenter = presenter;
        //Debug.Log("JHDHDKSGHSKF");
        if (settings.isBasicSettingsChange)
        {
            colorDuration = settings.colorDurationSeconds;
            scaleUpDuration = settings.scaleUpDurationSeconds;
            waitDuration = settings.waitDurationSeconds;
            scaleDownDuration = settings.scaleDownDurationSeconds;

            height = settings.height;
            offset = settings.offset;

            material = settings.material;
        }
        else
        {
            float bpm = settings.bpm;
            colorDuration = presenter.GetColorDurationSeconds(bpm);
            scaleUpDuration = presenter.GetScaleUpDurationSeconds(bpm);
            waitDuration = presenter.GetWaitDurationSeconds(bpm);
            scaleDownDuration = presenter.GetScaleDownDurationSeconds(bpm);

            height = presenter.GetHeight();
            offset = presenter.GetOffset();

            material = presenter.GetMaterial();
        }

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTime = !isTime;
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
        timer = 0f;
        state = State.Coloring;
        faceState.Set(FaceProperty.IsColored, true);
    }

    private void UpdateColoring()
    {
        ApplyRedFaceMaterial();
        AdvanceTimer();

        if (TimerExpired(colorDuration))
            StartScaleUp();
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
        faceScript.rend.material = material;        //CHANGE TO PRESENTER
        faceState.Set(FaceProperty.IsKilling, false);
        state = State.Done;
    }

    private void UpdateScaling(
        Vector3 fromScale, Vector3 toScale,
        Vector3 fromPos, Vector3 toPos,
        float duration, System.Action onComplete)
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

    /*
    public override void SetSettings<T>(T settings)
    {
        if (settings is not RedFaceSettings redFaceSettings)
        {
            Debug.LogError("WRONG SETTINGS FOR RED FACE SPAWNER");
            return;
        }

        bpm = redFaceSettings.bpm;

        isRandomSpawn = redFaceSettings.isRandom;

        //isResetAfterDeath  = redFaceSettings.isResetAfterDeath;

        if (isRandomSpawn)
        {

            isStableQuantity = redFaceSettings.isStableQuantity;
            if (isStableQuantity)
            {
                quantity = redFaceSettings.quantityExact;
            }
            else
            {
                quantityMin = redFaceSettings.quantityMin;
                quantityMax = redFaceSettings.quantityMax;
            }
        }

        isCertainSpawn = redFaceSettings.isCertain;

        if (isCertainSpawn)
        {
            isRelativeToFigure = redFaceSettings.isRelativeToFigure;
            isRelativeToPlayer = redFaceSettings.isRelativeToPlayer;

            if (isRelativeToFigure)
            {
                arrayOfFacesRelativeToFigure = redFaceSettings.arrayOfFacesRelativeToFigure;
            }

            if (isRelativeToPlayer)
            {
                arrayOfFacesRelativeToPlayer = redFaceSettings.arrayOfFacesRelativeToPlayer;
            }
        }

        isProximityLimit = redFaceSettings.isProximityLimit;

        if (isProximityLimit)
        {
            proximityLimit = redFaceSettings.proximityLimit;
        }

        isDistanceLimit = redFaceSettings.isDistanceLimit;

        if (isDistanceLimit)
        {
            distanceLimit = redFaceSettings.distanceLimit;
        }

        isBasicSettingsChange = redFaceSettings.isBasicSettingsChange;

        if (isBasicSettingsChange)
        {
            isMaterialChange = redFaceSettings.isMaterialChange;

            if (isMaterialChange)
            {
                //PRESENTER
            }

            isColorDurationChange = redFaceSettings.isColorDurationChange;

            if (isColorDurationChange)
            {
                colorDurationSeconds = redFaceSettings.colorDurationSeconds;
            }

            isScaleUpDurationChange = redFaceSettings.isScaleUpDurationChange;

            if (isScaleUpDurationChange)
            {
                scaleUpDurationSeconds = redFaceSettings.scaleUpDurationSeconds;
            }

            isWaitDurationChange = redFaceSettings.isWaitDurationChange;

            if (isWaitDurationChange)
            {
                waitDurationSeconds = redFaceSettings.waitDurationSeconds;
            }

            isScaleDownDurationChange = redFaceSettings.isScaleDownDurationChange;

            if (isScaleDownDurationChange)
            {
                scaleDownDurationSeconds = redFaceSettings.scaleDownDurationSeconds;
            }

            isHeightChange = redFaceSettings.isHeightChange;

            if (isHeightChange)
            {
                scaleChange = redFaceSettings.height;
            }

            isOffsetChange = redFaceSettings.isOffsetChange;

            if (isOffsetChange)
            {
                offset = redFaceSettings.offset;
            }
        }
        else
        {
            colorDurationSeconds = 60f / bpm * 3;
            scaleUpDurationSeconds = 60f / bpm / 2;
            waitDurationSeconds = 0f;
            scaleDownDurationSeconds = 60f / bpm;
            height = 60f;
            offset = height * 0.0009f;
        }


        isTurnOn = true;
    }
     * */
}