using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedFaceSpawnerScript : SpawnerActionScript
{
    public bool isTurnOn = false;
    private float colorChangeDuration;
    private float scaleChangeDurationUp;
    private float waitDuration;
    private float scaleChangeDurationDown;
    [SerializeField] private float scaleChange = 25f;
    [SerializeField] private float positionChange;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private PlayerScript PS;

    private void Start()
    {
        isTurnOn = false;
        colvo = 3;
        isRandomSpawnTime = true;
        //positionChange = scaleChange * -0.05f; // Rewrite
        faces = FAS.GetAllFaces();

    }

    public override void SetSettings<T>(T settings)
    {
        if (settings is not RedFaceSettings redSettings)
        {
            Debug.LogError("WRONG SETTINGS FOR RED FACE SPAWNER");
            return;
        }
        isRandomSpawnTime = redSettings.isRandom;

        colorChangeDuration = 60f / redSettings.bpm * 3;
        scaleChangeDurationUp = 60f / redSettings.bpm / 2;
        waitDuration = 0f;
        scaleChangeDurationDown = 60f / redSettings.bpm;

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
        Debug.Log("Here");
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

        if (faceState.Get(FaceProperty.HavePlayer)) { PS.SetPartsMaterial(materialPlayer); }
        else FS.rend.material = materialWhite;

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
