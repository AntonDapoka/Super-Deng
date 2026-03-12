using UnityEngine;

[CreateAssetMenu(fileName = "RedFaceBasicSettings", menuName = "ScriptableObjects/RedFaceBasicSettings", order = 2)]
public class RedFaceBasicSettings : ActionBasicSettingsScript
{
    public float heightBasic;
    public float offsetBasic;

    public int proximityLimit;
    public int distanceLimit;

    public float colorDurationSecondsBasic;
    public float scaleUpDurationSecondsBasic;
    public float waitDurationSecondsBasic;
    public float scaleDownDurationSecondsBasic;

    public Material materialBasic;
}
