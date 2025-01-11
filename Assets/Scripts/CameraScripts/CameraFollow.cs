using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool isTurnOn = true;
    [SerializeField] private Transform player; 
    [SerializeField] private Transform center; 
    [SerializeField] private float followSpeed = 5f; 

    private void Start()
    {
        transform.position = player.position;
        Vector3 direction = center.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void LateUpdate()
    {
        if (isTurnOn)
        {
            Vector3 targetPosition = player.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            Vector3 direction = center.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}