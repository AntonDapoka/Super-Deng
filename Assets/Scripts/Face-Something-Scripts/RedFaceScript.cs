using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RedFaceScript : MonoBehaviour
{
    public bool isTurnOn = false;
    private GameObject[] faces;
    private float colorChangeDuration;
    private float scaleChangeDurationUp;
    private float waitDuration;
    private float scaleChangeDurationDown;
    [SerializeField] private float scaleChange = 25f;
    private float positionChange;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private RhythmManager RM;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private ComboManager CM;

    public List<int> faceIndices = new();
    public int colvo;
    public bool isRandomSpawnTime = true;


    private void Start()
    {
        isTurnOn = false;
        positionChange = scaleChange * -0.05f;
        faces = FAS.GetAllFaces();
        SetBPMSettings();
    }

    public void StartSettingRedFace()
    {
        if (isTurnOn)
        {
            List<int> availableFaces = new List<int>(); //Составляем массив из доступных граней

            for (int i = 0; i < faces.Length; i++)
            {
                FaceScript FS = faces[i].GetComponent<FaceScript>();
                if (//!FS.havePlayer &&
                    !FS.isBlinking &&
                    !FS.isKilling &&
                    !FS.isBlocked &&
                    !FS.isColored &&
                    !FS.isPortal &&
                    !FS.isBonus)
                {
                    availableFaces.Add(i);
                }
            }

            if (isRandomSpawnTime)
            {
                //Debug.Log(colvo);
                for (int i = 0; i < colvo; i++)
                {
                    if (availableFaces.Count == 0) return;

                    int randomIndex = Random.Range(0, availableFaces.Count);
                    int selectedFaceIndex = availableFaces[randomIndex];
                    StartCoroutine(SetRedFace(faces[selectedFaceIndex])); //Запускаем рандомные из доступных
                    availableFaces.RemoveAt(randomIndex);
                }
            }
            else
            {
                var intersectedIndices = faceIndices.Intersect(availableFaces);
                foreach (int index in intersectedIndices)
                {
                    StartCoroutine(SetRedFace(faces[index])); //Запускаем указанные из доступных
                    availableFaces.RemoveAt(index);
                }
            }
        }
    }

    private IEnumerator SetRedFace(GameObject face)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        FaceDanceScript FDC = face.GetComponent<FaceDanceScript>();
        
        FDC.isTurnOn = false;
        FS.isColored = true;
        float timer = 0f;
        while (timer < colorChangeDuration)
        {
            FS.rend.material = materialRed;
            if (FS.havePlayer) PS.SetPartsMaterial(materialRed);
            timer += Time.deltaTime;
            yield return null;
        }
        FS.isKilling = true;
        FS.isColored = false;
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), new Vector3(0f, positionChange, 0f), scaleChangeDurationUp));

        yield return new WaitForSeconds(waitDuration);

        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), scaleChangeDurationDown)); 

        if (FS.havePlayer) { PS.SetPartsMaterial(materialPlayer); }
        else if (FS.isRight) FS.rend.material = FS.materialRightFace;
        else if (FS.isLeft) FS.rend.material = FS.materialLeftFace;
        else if (FS.isTop) FS.rend.material = FS.materialTopFace;
        else FS.rend.material = materialWhite;

        FS.isKilling = false;

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
            if (FS.havePlayer) PS.SetPartsMaterial(materialRed);

            FS.glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            FS.glowingPart.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }
        FS.glowingPart.transform.localScale = targetScale;
    }

    private void SetBPMSettings() 
    {
        colorChangeDuration = RM.beatInterval * 3;
        scaleChangeDurationUp = RM.beatInterval / 2;
        waitDuration = 0f;
        scaleChangeDurationDown = RM.beatInterval;
    }
}
