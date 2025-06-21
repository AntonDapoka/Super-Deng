using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveAndLoadIcosahedronCoordinatesDataScript : MonoBehaviour
{
    [SerializeField] private GameObject prefabFace;
    [SerializeField] private GameObject icosahedron;
    private IDataServiceScript dataService = new JsonDataServiceScript();
    public IcosahedronCoordinatesData �oordinatesData = new IcosahedronCoordinatesData();
    private readonly bool isEncrypted;

    private string relativePath = "/icosahedroncoordinates-datafile.json";

    public void SaveIcosahedronCoordinatesData()
    {
        FillSettingsSaveData();
        SerializeJson();
    }

    public void LoadIcosahedronCoordinatesData()
    {
        DeserializeJson();
        ReadSettingsSaveData();
    }

    private void SerializeJson()
    {
        long s = DateTime.Now.Ticks;
        long f = 0;
        if (dataService.SaveData(relativePath, �oordinatesData, isEncrypted))
        {
            f = DateTime.Now.Ticks - s;
            Debug.Log($"Save Time {(f / 100000f):N4}ms");
            Debug.Log(Application.persistentDataPath + relativePath);
        }
        else
        {
            Debug.LogError("Cant save file bro");
        }
    }

    private void DeserializeJson()
    {
        long s = DateTime.Now.Ticks;
        long f = 0;
        try
        {
            �oordinatesData = dataService.LoadData<IcosahedronCoordinatesData>(relativePath, isEncrypted);
            f = DateTime.Now.Ticks - s;
            Debug.Log($"Load Time {(f / 100000f):N4}ms");
        }
        catch
        {
            Debug.LogError("Cant load file bro");
        }
    }

    private void FillSettingsSaveData()
    {
        �oordinatesData = new IcosahedronCoordinatesData();

        if (icosahedron == null)
        {
            Debug.LogError("Icosahedron �� ��������!");
            return;
        }

        int childCount = icosahedron.transform.childCount;

        if (childCount != 20)
        {
            Debug.LogError("������ � ����������� ������");
            return;
        }

        �oordinatesData.positions = new SerializableVector3[childCount];
        �oordinatesData.rotations = new SerializableQuaternion[childCount];
        �oordinatesData.scales = new SerializableVector3[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = icosahedron.transform.GetChild(i);

            �oordinatesData.positions[i] = new SerializableVector3(child.position);
            �oordinatesData.rotations[i] = new SerializableQuaternion(child.rotation);
            �oordinatesData.scales[i] = new SerializableVector3(child.localScale);
        }

        Debug.Log("������ � �������� �������� �������.");
    }

    private void ReadSettingsSaveData()
    {
        if (�oordinatesData.positions == null || �oordinatesData.rotations == null || �oordinatesData.scales == null)
        {
            Debug.LogError("������������ ������ � �����.");
            return;
        }

        int count = �oordinatesData.positions.Length;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefabFace);

            obj.transform.position = �oordinatesData.positions[i].ToVector3();
            obj.transform.rotation = �oordinatesData.rotations[i].ToQuaternion();
            obj.transform.localScale = �oordinatesData.scales[i].ToVector3();
        }

        Debug.Log($"������� {count} ��������.");
    }
}


[System.Serializable]
public class IcosahedronCoordinatesData
{
    public SerializableVector3[] positions;
    public SerializableQuaternion[] rotations;
    public SerializableVector3[] scales;
}