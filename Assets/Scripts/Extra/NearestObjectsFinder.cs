using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class NearestObjectsFinder : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        FindThreeNearestObjects();
    }

    void FindThreeNearestObjects()
    {
        // ����� ��� ������� � ����������� FaceScript
        FaceScript[] allFaceScripts = FindObjectsOfType<FaceScript>();

        // �������� �� ������� �������� � FaceScript
        if (allFaceScripts.Length == 0)
        {
            Debug.Log("No objects with FaceScript found");
            return;
        }

        // ������� ������� �������, � �������� ���������� �����
        Vector3 currentPosition = transform.position;

        // ���������� �������� �� ���������� �� �������� �������
        var sortedObjects = allFaceScripts
            .OrderBy(faceScript => Vector3.Distance(currentPosition, faceScript.transform.position))
            .ToList();

        // ��������� ���� ��������� ��������
        var nearestObjects = sortedObjects.Take(3);

        // ����� ���������� � ��������� �������� � �������
        foreach (var obj in nearestObjects)
        {
            Debug.Log($"Found object {obj.gameObject.name} at distance {Vector3.Distance(currentPosition, obj.transform.position)}");
        }
    }
}
