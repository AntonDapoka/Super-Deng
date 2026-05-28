using UnityEngine;

[CreateAssetMenu(fileName = "BonusFaceBasicSettings", menuName = "ScriptableObjects/BonusFaceBasicSettings", order = 8)]
public class BonusFaceBasicSettings : ActionBasicSettingsScript
{
    public Material materialHealthBasic;
    public GameObject bonusHealthPrefabBasic;

    public Material materialComboBasic;
    public GameObject bonusComboPrefabBasic;

    public Material materialShieldBasic;
    public GameObject bonusShieldPrefabBasic;

    public Material materialSlowMoBasic;
    public GameObject bonusSlowMoPrefabBasic;

    public Material materialSpeedBasic;
    public GameObject bonusSpeedPrefabBasic;
}
