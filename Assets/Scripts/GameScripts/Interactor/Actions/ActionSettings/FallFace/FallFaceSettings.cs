using UnityEngine;

[CreateAssetMenu(fileName = "FallFaceSettings", menuName = "ScriptableObjects/FallFaceSettings", order = 5)]
public class FallFaceSettings : ActionSpawnerSettingsScript
{
    /// <summary>
    /// Don't mess up with the stuff below
    /// </summary>

    public bool isMaterialChange;
    public Material material;

    public bool isBlinkingDurationChange;
    public float blinkingDurationBeats;
    public float blinkingDurationSeconds;

    public bool isReturningDurationChange;
    public float returningDurationBeats;
    public float returningDurationSeconds;

    public bool isImpulseChange;
    public float impulse;
}
