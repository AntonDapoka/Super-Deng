using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimeManagementScript : MonoBehaviour
{
    [SerializeField] private bool isTurnOn = false;
    private float time;

    private void Awake()
    {
        time = 0f;
    }

    private void Update()
    {
        if (isTurnOn)
            time += Time.deltaTime;
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }

    public float GetCurrentTime()
    {
        return time;
    }

    public void ResetTime()
    {
        time = 0f;
    }
}
