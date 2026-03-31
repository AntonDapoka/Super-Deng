using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceArrayScript : MonoBehaviour
{
    [SerializeField] private GameObject[] faces;

    public void SetAllFaces(GameObject[] newFaces)
    {
        faces = newFaces
        .OrderBy(face => face.GetComponent<FaceScript>().GetFaceID())
        .ToArray();
    }

    public GameObject GetFaceByID(int id)
    {
        if (faces == null || id < 0 || id >= faces.Length)
            return null;

        return faces[id];
    }

    public GameObject[] GetAllFaces()
    {
        return faces;
    }

    public GameObject GetRandomFace()
    {
        return faces[Random.Range(0, faces.Length)];
    }

    public FaceScript[] GetAllFaceScripts()
    {
        FaceScript[] result = new FaceScript[faces.Length];
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i] != null)
            {
                result[i] = faces[i].GetComponent<FaceScript>();
            }
        }
        return result;
    }
}
