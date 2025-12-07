using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedFaceSpawnerScript : SpawnerActionScript
{
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

    private bool isColorDurationBeatsChange;
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

    private float colorChangeDuration;
    private float scaleChangeDurationUp;
    private float waitDuration;
    private float scaleChangeDurationDown;
    [SerializeField] private float scaleChange = 25f;
    [SerializeField] private float positionChange;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private RedFaceSpawnerPresenterScript presenter;

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
        isCertainSpawn = redFaceSettings.isCertain;
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

        if (isCertainSpawn)
        {

        }

        colorChangeDuration = 60f / bpm * 3;
        scaleChangeDurationUp = 60f / bpm / 2;
        waitDuration = 0f;
        scaleChangeDurationDown = 60f / bpm;

        isTurnOn = true;
    }

    public override bool IsSuitableSpecialRequirements()
    {
        return true;
    }

    public override void SetActionFace(GameObject gameObject)
    {
        if (isTurnOn)
            StartCoroutine(SetRedFace(gameObject));
    }

    private IEnumerator SetRedFace(GameObject face)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FaceStateScript faceState = face.GetComponent<FaceStateScript>();
        faceState.Set(FaceProperty.IsColored, true); 
        float timer = 0f;
        while (timer < colorChangeDuration)
        {
            FS.rend.material = materialRed;
            //if (faceState.havePlayer) PS.SetPartsMaterial(materialRed); // Commented out - field is commented in FaceScript
            timer += Time.deltaTime;
            yield return null;
        }
        faceState.Set(FaceProperty.IsKilling, true); 
        faceState.Set(FaceProperty.IsColored, false); 
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), new Vector3(0f, positionChange, 0f), scaleChangeDurationUp));

        yield return new WaitForSeconds(waitDuration);

        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), scaleChangeDurationDown));

        FS.rend.material = materialWhite;

        FS.rend.material = materialWhite; // Fallback
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
