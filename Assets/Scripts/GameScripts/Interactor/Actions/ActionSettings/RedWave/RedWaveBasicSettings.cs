using UnityEngine;

[CreateAssetMenu(fileName = "RedWaveBasicSettings", menuName = "ScriptableObjects/RedWaveBasicSettings", order = 4)]
public class RedWaveBasicSettings : ScriptableObject
{
    public Material material;

    public float height;
    public float offset;
}
