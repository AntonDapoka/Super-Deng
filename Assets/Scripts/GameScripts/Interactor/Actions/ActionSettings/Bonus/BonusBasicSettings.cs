using UnityEngine;

[CreateAssetMenu(fileName = "BonusBasicSettings", menuName = "ScriptableObjects/BonusBasicSettings", order = 8)]
public class BonusBasicSettings : ScriptableObject
{
    public Material material;

    public float height;
    public float offset;
}
