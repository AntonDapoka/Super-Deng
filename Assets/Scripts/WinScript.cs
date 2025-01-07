using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScript : MonoBehaviour
{
    [SerializeField] private Image imageCompleted;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private StartCountDown SCD;

    public void Win()
    {
        imageCompleted.gameObject.SetActive(true);
        RFS.isTurnOn = false;
    }
}
