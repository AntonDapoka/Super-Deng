using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcoSphereDanceScript : MonoBehaviour
{
    public GameObject[] objects;  // ������ �������� ��� ��������
    public float rotationAngle = 15f;  // ���� �������� � ��������
    public float duration = 0.2f;  // ����� ��� ���������� �������� � ���� �������
    public bool isOn = false;  // ���������� ��� ���������� ���������
    private bool inProcess = false;
    private int side = -1;
    [SerializeField] private EnemySpawnSettings enemySpawnSettings;
    [SerializeField] private TimerController TC;
    private bool[] spawnExecuted;

    private void Start()
    {
        spawnExecuted = new bool[enemySpawnSettings.spawnTimes.Length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isOn = !isOn;
        }
        float elapsedTime = TC.timeElapsed;

        for (int i = 0; i < enemySpawnSettings.spawnTimes.Length - 1; i++)
        {
            var spawnTimeData = enemySpawnSettings.spawnTimes[i];
            var nextSpawnTimeData = enemySpawnSettings.spawnTimes[i + 1];

            // ���������, ������ �� ��������� ����� � �� ��� �� ����� ��� ��������
            if (elapsedTime >= spawnTimeData.time && elapsedTime <= nextSpawnTimeData.time && !spawnExecuted[i])
            {
                //isOn = spawnTimeData.isSphereDance;
                spawnExecuted[i] = true;
            }
        }
        if (isOn && !inProcess)
        {
            foreach (GameObject obj in objects)
            {
                StartCoroutine(RotateObject(obj, rotationAngle, duration));
            }
        }
    }

    private IEnumerator RotateObject(GameObject obj, float angle, float time)
    {
        if (isOn)
        {
            inProcess = true;
            Quaternion originalRotation = obj.transform.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, side * angle, 0);

            // �������� �����
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.rotation = targetRotation;

            // �������� �������
            elapsedTime = 0f;
            while (elapsedTime < time)
            {
                obj.transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            obj.transform.rotation = originalRotation;
            inProcess = false;
            side = -side;
        }
        else
        {
            yield return null;
        }
    }
}