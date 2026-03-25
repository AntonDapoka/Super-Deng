using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCounterScript : MonoBehaviour
{
    private FaceScript[] faces;
    [SerializeField] private FaceArrayScript FAS;
    Queue<FaceScript> queue = new();

    public void StartPathCount()
    {
        faces = FAS.GetAllFaceScripts();
        SetPathCount();
    }

    public void SetPathCount()
    {
        FaceScript startface = null;
        
        foreach (var face in faces)
        {
            if (face.FaceState.Get(FaceProperty.HavePlayer))
            {
                startface = face;
            }
            face.PathObjectCount = -1;
        }
        queue = new Queue<FaceScript>();
        if (startface != null)
        {
            queue.Enqueue(startface);
            startface.PathObjectCount = 0;
        }
        while (queue.Count > 0)
        {
            FaceScript current = queue.Dequeue();

            FaceScript[] neighbors = { current.side1.GetComponent<FaceScript>(), 
                current.side2.GetComponent<FaceScript>(),
                current.side3.GetComponent<FaceScript>() };
            foreach (var neighbor in neighbors)
            {
                if (neighbor != null && neighbor.PathObjectCount == -1)
                {
                    neighbor.PathObjectCount = current.PathObjectCount + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }
    }
}
