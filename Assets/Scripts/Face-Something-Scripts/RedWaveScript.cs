using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedWaveScript : MonoBehaviour
{
    public bool isTurnOn = false;
    private GameObject[] faces;
    public int proximityLimit = 5;
    public float lifeDuration = 4f;
    private float colorChangeDuration;
    private float scaleChangeDurationUp;
    private float waitDuration;
    private float scaleChangeDurationDown;
    [SerializeField] private float scaleChange = 4f;
    [SerializeField] private float positionChange = -4f;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialRed;
    [SerializeField] private Material materialPlayer;
    [SerializeField] private RhythmManager RM;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private TimerController TC;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private ComboManager CM;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    

    public List<int> faceIndices = new();
    public int colvo = 0;
    public bool isRandomSpawnTime = false;

    private List<WaveCall> waveOrder = new List<WaveCall>();
    private int waveCount = 0;

    private void Start()
    {
        faces = FAS.GetAllFaces();
        isRandomSpawnTime = false;
        isTurnOn = false;
        SetBPMSettings();
    }

    private void Update()
    {
        CheckAndRemoveExpiredWaves();
    }

    public void StartSettingRedWave()
    {
        if (isTurnOn)
        {
            List<int> availableFaces = new List<int>();

            for (int i = 0; i < faces.Length; i++)
            {
                FaceScript FS = faces[i].GetComponent<FaceScript>();
                if (!FS.havePlayer &&
                    !FS.isBlinking &&
                    !FS.isKilling &&
                    !FS.isBlocked &&
                    !FS.isColored &&
                    !FS.isPortal &&
                    !FS.isBonus &&
                    FS.pathObjectCount >= proximityLimit)
                {
                    availableFaces.Add(i);
                }
            }

            if (isRandomSpawnTime)
            {
                for (int i = 0; i < colvo; i++)
                {
                    if (availableFaces.Count == 0) return;

                    waveCount++; // Увеличиваем счётчик вызовов
                    float removalTime = Time.time + lifeDuration; // Время, когда нужно удалить вызов
                    waveOrder.Add(new WaveCall(waveCount, removalTime)); // Добавляем вызов в список

                    int randomIndex = Random.Range(0, availableFaces.Count);
                    int selectedFaceIndex = availableFaces[randomIndex];
                    StartCoroutine(SetRedWave(faces[selectedFaceIndex], waveCount));
                    availableFaces.RemoveAt(randomIndex);
                }
            }
            else
            {
                var intersectedIndices = faceIndices.Intersect(availableFaces);
                foreach (int index in intersectedIndices)
                {
                    waveCount++; // Аналогично
                    float removalTime = Time.time + lifeDuration; 
                    waveOrder.Add(new WaveCall(waveCount, removalTime)); 

                    StartCoroutine(SetRedWave(faces[index], waveCount));
                    availableFaces.RemoveAt(index);
                }
            }
        }
    }

    private IEnumerator SetRedWave(GameObject face, int callNumber)
    {
        float startTime = Time.time;

        FaceScript FS = face.GetComponent<FaceScript>();
        FaceDanceScript FDC = face.GetComponent<FaceDanceScript>();
        
        FDC.isTurnOn = false;
        float timer = 0f;
        while (timer < colorChangeDuration)
        {
            SetMaterial(FS, materialRed);
            timer += Time.deltaTime;
            yield return null;
        }

        FS.isKilling = true;

        if (TC.isTurnOn && waveOrder.Exists(call => call.CallNumber == callNumber))
        {
            SetNextStep(FS, callNumber);
        }
        
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, scaleChange), new Vector3(0f, positionChange, 0f), scaleChangeDurationUp, true));
        
        yield return new WaitForSeconds(waitDuration);
       
        yield return StartCoroutine(ChangeScale(face, new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), scaleChangeDurationDown, false));

        SetMaterialBack(FS);

        FS.isKilling = false;
    }

    private IEnumerator ChangeScale(GameObject face, Vector3 targetScale, Vector3 targetPosition, float duration, bool flag)
    {
        FaceScript FS = face.GetComponent<FaceScript>();
        Vector3 startScale = FS.glowingPart.transform.localScale;
        Vector3 startPosition = FS.glowingPart.transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            SetMaterial(FS, materialRed);
            if (flag)
            {
                FS.glowingPart.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), targetScale, timer / duration);
            }
            else
            {
                FS.glowingPart.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            }
            
            FS.glowingPart.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }
        FS.glowingPart.transform.localScale = targetScale;
    }

    private void SetMaterial(FaceScript FS, Material material)
    {
        if (!FS.havePlayer) FS.rend.material = material;
        else PS.SetPartsMaterial(material);
    }

    private void SetMaterialBack(FaceScript FS)
    {
        if (FS.havePlayer) PS.SetPartsMaterial(materialPlayer); 
        else if (FS.isRight) FS.rend.material = FS.materialRightFace;
        else if (FS.isLeft) FS.rend.material = FS.materialLeftFace;
        else if (FS.isTop) FS.rend.material = FS.materialTopFace;
        else FS.rend.material = materialWhite;
    }

    private void SetNextStep(FaceScript facescript, int callNumber)
    {
        FaceScript objectWithMinPathCounter = null;
        int minPathCounter = int.MaxValue;

        FaceScript[] faces = { facescript.FS1, facescript.FS2, facescript.FS3 };

        foreach (FaceScript face in faces)
        {
            if (face == null) continue;

            if (face != null && (face.pathObjectCount < minPathCounter))
            {
                if (!face.isBlinking &&
                    !face.isKilling &&
                    !face.isBlocked &&
                    !face.isColored &&
                    !face.isPortal &&
                    !face.isBonus)
                {
                    minPathCounter = face.pathObjectCount;
                    objectWithMinPathCounter = face;
                }
            }
        }
        if (facescript.pathObjectCount != 0 && objectWithMinPathCounter != null)
        {
            StartCoroutine(SetRedWave(objectWithMinPathCounter.gameObject, callNumber));
        }
            
    }

    private void SetBPMSettings()
    {
        colorChangeDuration = RM.beatInterval;
        scaleChangeDurationUp = RM.beatInterval;
        waitDuration = RM.beatInterval;
        scaleChangeDurationDown = RM.beatInterval;
    }
    private void CheckAndRemoveExpiredWaves()
    {
        float currentTime = Time.time; // Текущее время
        for (int i = waveOrder.Count - 1; i >= 0; i--) // Обратный цикл для безопасного удаления
        {
            if (waveOrder[i].RemovalTime <= currentTime)
            {
                waveOrder.RemoveAt(i);
            }
        }
    }

    private struct WaveCall
    {
        public int CallNumber { get; } // Номер вызова
        public float RemovalTime { get; } // Время удаления

        public WaveCall(int callNumber, float removalTime)
        {
            CallNumber = callNumber;
            RemovalTime = removalTime;
        }
    }
}
