using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseToTheBeat : MonoBehaviour
{
    [SerializeField] private float pulseSize = 1.5f;
    [SerializeField] private float returnSpeed = 10f;
    private Vector3 startSize;

    private void Start()
    {
        startSize = transform.localScale;
    }
    
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
    }

    public void Pulse()
    {
        transform.localScale = startSize * pulseSize;
    }

}
