using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceSetterScript : MonoBehaviour
{
    /* Triangle orientation:
      
                      /\  
                     /  \
                    /  3 \
                   /______\
                  / \    / \
                 / 1 \  / 2 \
                /_____\/_____\
    */

    [SerializeField] private GameObject[] faces;
    [SerializeField] private FaceArrayScript faceArrayScript;

    private void Start()
    {
        InitializeAllFaces();
    }

    public void InitializeAllFaces()
    {
        faces = faceArrayScript.GetAllFaces();
        foreach (GameObject face in faces)
        {
            FaceScript faceScript = face.GetComponent<FaceScript>();
            GameObject[] closestObjects = FindClosestObjectsFromArray(faces, face.transform, 3);
            faceScript.Initialize(closestObjects, false);
        }
    }

    GameObject[] FindClosestObjectsFromArray(GameObject[] objectsArray, Transform from, int count)
    {
        return objectsArray
            .OrderBy(obj => Vector3.Distance(from.position, obj.transform.position))
            .Skip(1) // skip "itself" if it is in the array
            .Take(count)
            .ToArray();
    }
}
