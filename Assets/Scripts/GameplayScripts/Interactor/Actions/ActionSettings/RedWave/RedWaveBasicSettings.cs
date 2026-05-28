using UnityEngine;

[CreateAssetMenu(fileName = "RedWaveBasicSettings", menuName = "ScriptableObjects/RedWaveBasicSettings", order = 4)]
public class RedWaveBasicSettings : ActionBasicSettingsScript
{
    public Material materialBasic;

    public int proximityLimit;
    public int distanceLimit;

    public float colorDurationSecondsBasic;
    public float scaleUpDurationSecondsBasic;
    public float waitDurationSecondsBasic;
    public float scaleDownDurationSecondsBasic;

    public float heightBasic;
    public float offsetBasic;
}
