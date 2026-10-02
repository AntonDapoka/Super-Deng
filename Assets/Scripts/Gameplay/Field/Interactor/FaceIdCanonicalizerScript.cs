using System.Collections.Generic;
using UnityEngine;

public static class FaceIdCanonicalizerScript
{
    public static void AssignIds(List<(GameObject face, long rawKey)> entries)
    {
        if (entries == null || entries.Count == 0)
            return;

        entries.Sort((a, b) => a.rawKey.CompareTo(b.rawKey));

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject face = entries[i].face;
            if (face != null && face.TryGetComponent(out FaceScript faceScript))
            {
                faceScript.SetFaceID(i);
                faceScript.GetComponent<FaceIDViewScript>().DisplayID(i.ToString()); //Remove
            }
        }
    }
}
