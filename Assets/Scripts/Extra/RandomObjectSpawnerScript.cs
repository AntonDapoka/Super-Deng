using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawnerScript : MonoBehaviour
{
    public GameObject sphere;
    public GameObject[] sourceObjects;
    public FaceArrayScript FAS;
    public Vector2 distanceRange = new Vector2(1f, 5f); // ������� � �������� ���������� ����� Y
    public float offsetRatio = 0.2f; // 20% ����������

    void Start()
    {
        sourceObjects = FAS.GetAllFaces();

        foreach (GameObject source in sourceObjects)
        {
            // Commented out - field is commented in FaceScript
            //if (source == null || source.GetComponent<FaceScript>().havePlayer) continue;
            if (source == null) continue;

            float distance = Random.Range(distanceRange.x, distanceRange.y);

            Vector3 localY = -source.transform.up;

            Vector3 basePosition = source.transform.position + localY * distance;

            Vector3 randomDir = Random.insideUnitCircle.normalized; // � 2D
            Vector3 right = source.transform.right;
            Vector3 forward = source.transform.forward;
            Vector3 offset = (right * randomDir.x + forward * randomDir.y) * distance * offsetRatio;

            Vector3 finalPosition = basePosition + offset;

            GameObject spawned = new GameObject("OffsetPoint");
            spawned.transform.position = finalPosition;
            source.AddComponent<FaultTargetPointScript>();
            source.GetComponent<FaultTargetPointScript>().SetTarget(spawned);
        }
    }
}