using UnityEngine;

public class IcosphereLikeHandler : ISphereFigureHandler<IcosahedronCoordinatesData>
{

    public IcosahedronCoordinatesData CollectData(GameObject rootObject)
    {

        var data = new IcosahedronCoordinatesData();
        int stripCount = rootObject.transform.childCount;
        data.dataStrips = new StripHolderData[stripCount];

        for (int i = 0; i < stripCount; i++)
        {
            Transform strip = rootObject.transform.GetChild(i);
            var stripData = new StripHolderData();
            int faceCount = strip.childCount;
            stripData.transforms = new FaceTransformData[faceCount];

            for (int j = 0; j < faceCount; j++)
                stripData.transforms[j] = new FaceTransformData(strip.GetChild(j));

            data.dataStrips[i] = stripData;
        }

        return data;
    }

    public void ApplyData(GameObject prefab, IcosahedronCoordinatesData data, Transform parent)
    {
        foreach (var stripData in data.dataStrips)
        {
            GameObject stripObj = new GameObject("Strip");
            stripObj.transform.parent = parent;

            foreach (var transformData in stripData.transforms)
            {
                GameObject face = GameObject.Instantiate(prefab);
                transformData.ApplyTo(face.transform);
                face.transform.parent = stripObj.transform;
            }
        }
    }
}
