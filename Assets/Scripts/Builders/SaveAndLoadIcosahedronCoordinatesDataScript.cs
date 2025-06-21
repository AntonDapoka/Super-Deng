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
    public IcosahedronCoordinatesData ñoordinatesData = new IcosahedronCoordinatesData();
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
        if (dataService.SaveData(relativePath, ñoordinatesData, isEncrypted))
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
            ñoordinatesData = dataService.LoadData<IcosahedronCoordinatesData>(relativePath, isEncrypted);
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
        ñoordinatesData = new IcosahedronCoordinatesData();

        if (icosahedron == null)
        {
            Debug.LogError("Icosahedron íå íàçíà÷åí!");
            return;
        }

        int childCount = icosahedron.transform.childCount;

        if (childCount != 20)
        {
            Debug.LogError("Îøèáêà ñ êîëè÷åñòâîì ãðàíåé");
            return;
        }

        ñoordinatesData.positions = new SerializableVector3[childCount];
        ñoordinatesData.rotations = new SerializableQuaternion[childCount];
        ñoordinatesData.scales = new SerializableVector3[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = icosahedron.transform.GetChild(i);

            ñoordinatesData.positions[i] = new SerializableVector3(child.position);
            ñoordinatesData.rotations[i] = new SerializableQuaternion(child.rotation);
            ñoordinatesData.scales[i] = new SerializableVector3(child.localScale);
        }

        Debug.Log("Äàííûå î äî÷åðíèõ îáúåêòàõ ñîáðàíû.");
    }

    private void ReadSettingsSaveData()
    {
        if (ñoordinatesData.positions == null || ñoordinatesData.rotations == null || ñoordinatesData.scales == null)
        {
            Debug.LogError("Íåêîððåêòíûå äàííûå â ôàéëå.");
            return;
        }

        int count = ñoordinatesData.positions.Length;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefabFace);

            obj.transform.position = ñoordinatesData.positions[i].ToVector3();
            obj.transform.rotation = ñoordinatesData.rotations[i].ToQuaternion();
            obj.transform.localScale = ñoordinatesData.scales[i].ToVector3();
        }

        Debug.Log($"Ñîçäàíî {count} îáúåêòîâ.");
    }
}


[System.Serializable]
public class IcosahedronCoordinatesData
{
    public SerializableVector3[] positions;
    public SerializableQuaternion[] rotations;
    public SerializableVector3[] scales;
}