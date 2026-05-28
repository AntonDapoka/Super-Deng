using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexDistanceMeasureScript : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (pointA != null && pointB != null)
            {
                float distance = Vector3.Distance(pointA.position, pointB.position);
                Debug.Log("Distance: " + distance);
            }
            else
            {
                Debug.LogWarning("Assign both pointA and pointB in the Inspector.");
            }
        }
    }
}
