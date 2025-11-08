using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadIcosphereLikeFaceCoordinatesDataScript : MonoBehaviour
{
    [SerializeField] private int gradation;
    [SerializeField] private GameObject prefabFace;
    [SerializeField] private GameObject icosahedron;
    private SphereFigureDataService dataService = new SphereFigureDataService();
    private ISphereFigureHandler<IcosahedronCoordinatesData> icosahedronHandler = new IcosphereLikeHandler();

    private const string IcosahedronPath = "/icosahedroncoordinates-datafile.json";
    private const string Icosphere80Path = "/icosphere80coordinates-datafile.json";
    private const string Icosphere320Path = "/icosphere320coordinates-datafile.json";

    public void SaveIcosphereLike() ///int gradation
    {
        var data = icosahedronHandler.CollectData(icosahedron);
        string path = GetPath(gradation);
        dataService.Save(path, data);
    }

    public void LoadIcosphereLike() ///int gradation
    {
        string path = GetPath(gradation);
        var data = dataService.Load<IcosahedronCoordinatesData>(path);
        Transform parent = new GameObject("parent").transform;
        icosahedronHandler.ApplyData(prefabFace, data, parent);
    }

    private string GetPath(int gradation)
    {
        string path = IcosahedronPath;
        switch (gradation)
        {
            case 0:
                path = IcosahedronPath;
                break;
            case 1:
                path = Icosphere80Path;
                break;
            case 2:
                path = Icosphere320Path;
                break;
            default:
                path = "/icosphereGradation" + gradation + "coordinatesdatafile.json";
                break;
        }
        return path;
    }
}

[System.Serializable]
public class IcosahedronCoordinatesData : ISphereFigureData
{
    public StripHolderData[] dataStrips;
}

[System.Serializable]
public class StripHolderData
{
    public FaceTransformData[] transforms;
}

[System.Serializable]
public class FaceTransformData
{
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public SerializableVector3 scale;

    public FaceTransformData() { }

    public FaceTransformData(Transform t)
    {
        position = new SerializableVector3(t.position);
        rotation = new SerializableQuaternion(t.rotation);
        scale = new SerializableVector3(t.localScale);
    }

    public void ApplyTo(Transform t)
    {
        t.SetPositionAndRotation(position.ToVector3(), rotation.ToQuaternion());
        t.localScale = scale.ToVector3();
    }
}