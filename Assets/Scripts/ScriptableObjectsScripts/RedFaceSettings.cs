using UnityEngine;

[CreateAssetMenu(fileName = "RedFaceSettings", menuName = "ScriptableObjects/RedFaceSettings", order = 2)]
public class RedFaceSettings : ScriptableObject
{
    public string effectName;
    public int bpm;
    public float timeStartSeconds;
    public float timeStartBeats;
    public bool isTimeEnd;
    public float timeEndSeconds; //Only if isTimeEnd == true
    public float timeEndBeats; //Only if isTimeEnd == true
    public bool isRandom;
    public bool isArray;
    public bool isInterval; //Only if isRandom == true
    public int quantityExact; //Only if isRandom == true && isExactNumber == true
    public int quantityMin; //Only if isRandom == true && isExactNumber == false
    public int quantityMax; //Only if isRandom == true && isExactNumber == false
    public bool isCommonArray;
    public int[] arrayOfFaces;
    public bool isStripArray;
    public int[] arrayOfStrips;
    public bool isRelativeToThePlayerArray;
    public int[] arrayOfEqualDistanceFaces;
    public bool isTurnOnWayFinder;
    public float colorDurationBeatsChange;
    public float colorDurationSecondsChange;
    public float scaleDurationBeatsChangeUp;
    public float scaleDurationSecondsChangeUp;
    public float waitDurationBeats;
    public float waitDurationSeconds;
    public float scaleDurationBeatsChangeDown;
    public float scaleDurationSecondsChangeDown;
    public float scaleChange;

    

    
}
