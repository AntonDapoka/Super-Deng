using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    private GameObject[] faces;
    private FaceScript[] faceScripts;
    [SerializeField] private FaceArrayScript FAS;
    private List<FallData> fallDataList = new List<FallData>();
    public int proximityLimit = 0;
    public bool isResetDelay = false;
    [SerializeField] private PlayerScript PS;
    [SerializeField] private Vector3 centerPoint;
    [SerializeField] private float impulseForce = 10f; 
    [SerializeField] private float torqueStrength = 10f; 
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private AnimationClip animClipFall;
    [SerializeField] private AnimationClip animClipReset;
    public float resetDelayTime = 0f;
    private bool waitForDeath = false;
    public bool isTurnOn = false;
    public bool isRandomSpawnTime = false;
    public int colvo = 0;
    public List<int> faceIndices = new();

    private void Start()
    {
        faces = FAS.GetAllFaces();
        faceScripts = FAS.GetAllFaceScripts();

        foreach (GameObject face in faces)
        {
            if (face.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.useGravity = false;
            }
            if (face.TryGetComponent<Animator>(out var animator))
            {
                animator.enabled = false;
            }
        }
    }

    public void StartSettingFallFace()
    {
        if (fallDataList.Count >= 79) return;

        if (isTurnOn)
        {
            List<int> availableFaces = new List<int>();

            for (int i = 0; i < faces.Length; i++)
            {
                FaceScript FS = faces[i].GetComponent<FaceScript>();
                if (//!FS.havePlayer &&
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

                    int randomIndex = Random.Range(0, availableFaces.Count);
                    int selectedFaceIndex = availableFaces[randomIndex];

                    Rigidbody rb = faces[selectedFaceIndex].GetComponent<Rigidbody>();
                    rb.transform.GetPositionAndRotation(out Vector3 initialPosition, out var initialRotation);
                    rb.transform.GetLocalPositionAndRotation(out Vector3 initialLocalPosition, out var initialLocalRotation);
                    fallDataList.Add(new FallData(selectedFaceIndex, initialPosition, initialRotation, initialLocalPosition, initialLocalRotation));

                    StartCoroutine(PlayAnimationFall(faces[selectedFaceIndex], selectedFaceIndex));
                    availableFaces.RemoveAt(randomIndex);
                }
            }
            else
            {
                var intersectedIndices = faceIndices.Intersect(availableFaces);
                foreach (int index in intersectedIndices)
                {
                    Debug.Log(index);
                    Rigidbody rb = faces[index].GetComponent<Rigidbody>();
                    rb.transform.GetPositionAndRotation(out Vector3 initialPosition, out var initialRotation);
                    rb.transform.GetLocalPositionAndRotation(out Vector3 initialLocalPosition, out var initialLocalRotation);
                    fallDataList.Add(new FallData(index, initialPosition, initialRotation, initialLocalPosition, initialLocalRotation));

                    StartCoroutine(PlayAnimationFall(faces[index], index));
                    availableFaces.RemoveAt(index);
                }
            }
        }
    }

    private IEnumerator PlayAnimationFall(GameObject face, int numb)
    {
        FaceScript FS = face.GetComponent<FaceScript>();    
        Animator animator = face.GetComponent<Animator>();
        if (animator != null && animClipFall != null)
        {
            animator.enabled = true;
            FS.isBlinking = true;
            animator.Play(animClipFall.name);
            yield return new WaitForSeconds(animClipFall.length);
            FS.isBlinking = false;
            animator.enabled = false;
        }
        FS.isBlocked = true;
        ApplyImpulse(face, numb);
    }

    private void ApplyImpulse(GameObject face, int numb)
    {
        Rigidbody rb = face.GetComponent<Rigidbody>();

        if (face.GetComponent<FaceScript>().havePlayer) 
        {
            face.GetComponent<FaceScript>().havePlayer = false;
            waitForDeath = true;
        }

        Vector3 direction = (rb.transform.position - centerPoint).normalized;
        rb.AddForce(direction * impulseForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * torqueStrength;

        rb.AddTorque(randomTorque, ForceMode.Impulse);

        StartCoroutine(ResetAfterDelay(face, delay, numb, false));
    }

    IEnumerator ResetAfterDelay(GameObject face, float delay, int numb, bool isVisible)
    {
        Rigidbody rb = face.GetComponent<Rigidbody>();
        Renderer[] childRenderers = face.GetComponentsInChildren<Renderer>();

        yield return new WaitForSeconds(delay);

        if (waitForDeath)
        {
            PS.StartLosing();
        }

        foreach (Renderer renderer in childRenderers)
        {
            if (isVisible) renderer.enabled = true;
            else renderer.enabled = false;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //Debug.Log(fallDataList.Count);
        rb.transform.SetPositionAndRotation(fallDataList.First(data => data.FallFaceNumber == numb).InitialPosition, fallDataList.First(data => data.FallFaceNumber == numb).InitialLocalRotation);
        rb.transform.SetLocalPositionAndRotation(fallDataList.First(data => data.FallFaceNumber == numb).InitialLocalPosition, fallDataList.First(data => data.FallFaceNumber == numb).InitialLocalRotation);
        if (isVisible) 
        {
            StartCoroutine(PlayAnimationReset(face, numb));
            
        }
        else if (isResetDelay && !isVisible)
        {
            StartCoroutine(ResetAfterDelay(face, resetDelayTime, numb, true));
        }

    }

    public void ResetFall()
    {
        StopAllCoroutines();

        for (int i = 0; i < fallDataList.Count; i++)
        {
            //Debug.Log(fallDataList[i].FallFaceNumber);
            StartCoroutine(ResetAfterDelay(faces[fallDataList[i].FallFaceNumber], 0f, fallDataList[i].FallFaceNumber, true));
            faces[fallDataList[i].FallFaceNumber].GetComponent<FaceScript>().isBlinking = false;
        }
    }

    private IEnumerator PlayAnimationReset(GameObject face, int numb)
    {
        foreach (FaceScript FS in faceScripts)
        {
            FS.ResetRightLeftTop();
        }
        Animator animator = face.GetComponent<Animator>();
        animator.enabled = true;
        if (animator != null && animClipReset != null)
        {
            animator.enabled = true;
            animator.Play(animClipReset.name);
            yield return new WaitForSeconds(animClipReset.length);
        }
        foreach (FaceScript FS in faceScripts)
        {
            FS.ResetRightLeftTop();
        }
        animator.enabled = false;

        fallDataList.RemoveAll(f => f.FallFaceNumber == numb);
        face.GetComponent<FaceScript>().isBlocked = false;
    }


    private struct FallData
    {
        public int FallFaceNumber { get; } 
        public Vector3 InitialPosition { get; }
        public Quaternion InitialRotation { get; }
        public Vector3 InitialLocalPosition { get; }
        public Quaternion InitialLocalRotation { get; }

        public FallData(int fallFaceNumber, Vector3 initialPosition, Quaternion initialRotation, Vector3 initialLocalPosition, Quaternion initialLocalRotation)
        {
            FallFaceNumber = fallFaceNumber;
            InitialPosition = initialPosition;
            InitialRotation = initialRotation;
            InitialLocalPosition = initialLocalPosition;
            InitialLocalRotation = initialLocalRotation;
        }
    }

}
