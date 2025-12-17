using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RedFaceSpawnerScript : SpawnerActionScript
{
    private List<RedFaceScript> redFaces = new();

    private bool isCertainSpawn;
    private bool isResetAfterDeath;
    private bool isStableQuantity;
    private int quantityExact;
    private int quantityMin;
    private int quantityMax;

    private bool isRelativeToPlayer;
    private int[] arrayOfFacesRelativeToPlayer;
    private bool isRelativeToFigure;
    private int[] arrayOfFacesRelativeToFigure;

    private bool isProximityLimit;
    private int proximityLimit;
    private bool isDistanceLimit;
    private int distanceLimit;

    private bool isBasicSettingsChange;

    private bool isMaterialChange;
    private Material material;

    private bool isColorDurationChange;
    private float colorDurationBeats;
    private float colorDurationSeconds;

    private bool isScaleUpDurationChange;
    private float scaleUpDurationBeats;
    private float scaleUpDurationSeconds;

    private bool isWaitDurationChange;
    private float waitDurationBeats;
    private float waitDurationSeconds;

    private bool isScaleDownDurationChange;
    private float scaleDownDurationBeats;
    private float scaleDownDurationSeconds;

    private bool isHeightChange;
    private float height;

    private bool isOffsetChange;
    private float offset;

    [SerializeField] private float scaleChange = 25f;
    [SerializeField] private float positionChange;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private RedFaceSpawnerPresenterScript presenter;

    private FaceScript currentFaceScript;
    private FaceStateScript currentFaceState;

    State state = State.Idle;

    enum State
    {
        Idle,
        Coloring,
        ScaleUp,
        Waiting,
        ScaleDown
    }

    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        /*isTurnOn = false;
        quantity = 3;
        isRandomSpawn = true;*/
        //positionChange = scaleChange * -0.05f; // Rewrite
    }

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



    public override bool IsSuitableSpecialRequirements()
    {
        return true;
    }

    public override void SetActionFace(GameObject gameObject)
    {
        if (isTurnOn)
        {
            FaceScript faceScript = gameObject.GetComponent<FaceScript>();
            FaceStateScript faceState = faceScript.FaceState;
            faceState.Set(FaceProperty.IsColored, true);
            //timer = 0f;
            state = State.Coloring;
        }
        //StartCoroutine(SetRedFace(gameObject));
    }

    public void Update()
    {
        switch (state)
        {
            case State.Coloring: UpdateColoring(); break;
            case State.ScaleUp: UpdateScaleUp(); break;
            case State.Waiting: UpdateWait(); break;
            case State.ScaleDown: UpdateScaleDown(); break;
        }
    }

    private IEnumerator SetRedFace(GameObject face)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FaceStateScript faceState = face.GetComponent<FaceStateScript>();
        faceState.Set(FaceProperty.IsColored, true); 
        float timer = 0f;

        while (timer < colorDurationSeconds)
        {
            FS.rend.material = materialRed;
            timer += Time.deltaTime;
            yield return null;
        }

        faceState.Set(FaceProperty.IsKilling, true); 
        faceState.Set(FaceProperty.IsColored, false); 

        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), new Vector3(0f, positionChange, 0f), scaleUpDurationSeconds));

        yield return new WaitForSeconds(waitDurationSeconds);

        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), scaleDownDurationSeconds));

        FS.rend.material = materialWhite;

        faceState.Set(FaceProperty.IsKilling, false);

    }

    IEnumerator ChangeScale(GameObject face, Vector3 targetScale, Vector3 targetPosition, float duration)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        Vector3 startScale = FS.glowingPart.transform.localScale;
        Vector3 startPosition = FS.glowingPart.transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            FS.rend.material = materialRed;
            FS.glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            FS.glowingPart.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        FS.glowingPart.transform.localScale = targetScale;
        FS.glowingPart.transform.localPosition = targetPosition;
    }
}


