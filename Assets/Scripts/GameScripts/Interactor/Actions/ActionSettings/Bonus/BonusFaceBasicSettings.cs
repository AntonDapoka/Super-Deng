using UnityEngine;

[CreateAssetMenu(fileName = "BonusFaceBasicSettings", menuName = "ScriptableObjects/BonusFaceBasicSettings", order = 8)]
public class BonusFaceBasicSettings : ActionBasicSettingsScript
{
    public Material materialBasic;

    public float lifeDurationSecondsBasic;
    public float deathDurationSecondsBasic;
}
