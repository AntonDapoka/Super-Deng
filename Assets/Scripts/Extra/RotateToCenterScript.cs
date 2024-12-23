using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCenterScript : MonoBehaviour
{
    [SerializeField] private Transform centerObject; 
    [SerializeField] private Transform[] objectsToRotate;
    [SerializeField] private Vector3 additionalRotation; 

    private Vector3 previousRotation; 

    private void Update()
    {
        if (additionalRotation != previousRotation)
        {
            Rotate();
            previousRotation = additionalRotation;
        }
    }

    private void Rotate()
    {
        foreach (Transform obj in objectsToRotate)
        {
            if (obj == null)
                continue;
            Vector3 directionToCenter = (centerObject.position - obj.position).normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(obj.up, directionToCenter) * obj.rotation;
            Quaternion additionalQuat = Quaternion.Euler(additionalRotation);
            obj.rotation = targetRotation * additionalQuat;
        }
    }
}
