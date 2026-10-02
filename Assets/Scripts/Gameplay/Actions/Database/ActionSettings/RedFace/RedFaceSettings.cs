using UnityEngine;

[CreateAssetMenu(fileName = "RedFaceSettings", menuName = "ScriptableObjects/RedFaceSettings", order = 1)]
public class RedFaceSettings : ActionSpawnerSettingsScript
{
    /// <summary>
    /// Don't mess up with the stuff below
    /// </summary>

    public bool isMaterialChange;
    public Material material;

    public bool isColorDurationChange;
    public float colorDurationBeats;
    public float colorDurationSeconds;

    public bool isScaleUpDurationChange;
    public float scaleUpDurationBeats;
    public float scaleUpDurationSeconds;

    public bool isWaitDurationChange;
    public float waitDurationBeats;
    public float waitDurationSeconds;

    public bool isScaleDownDurationChange;
    public float scaleDownDurationBeats;
    public float scaleDownDurationSeconds;

    public bool isHeightChange;
    public float height;

    public bool isOffsetChange;
    public float offset;
}
