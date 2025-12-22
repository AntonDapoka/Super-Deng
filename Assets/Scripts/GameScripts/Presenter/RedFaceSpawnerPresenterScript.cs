using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFaceSpawnerPresenterScript : MonoBehaviour
{
    [SerializeField] private RedFaceBasicSettings redFaceBasicSettings;
    [SerializeField] private Material materialBasic;
    [SerializeField] private Material materialRedFace;
    [SerializeField] private Material materialPlayer;

    public float GetColorDurationSeconds(float bpm)
    {
        return bpm / 60f;
    }

    public float GetScaleUpDurationSeconds(float bpm)
    {
        return bpm / 60f;
    }

    public float GetWaitDurationSeconds(float bpm)
    {
        return 0f;
    }

    public float GetScaleDownDurationSeconds(float bpm)
    {
        return bpm / 60f;
    }

    public float GetHeight()
    {
        return redFaceBasicSettings.height;
    }

    public float GetOffset()
    {
        return redFaceBasicSettings.offset;
    }

    public Material GetMaterial()
    {
        return redFaceBasicSettings.material;
    }
}
