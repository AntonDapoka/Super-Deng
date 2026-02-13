using UnityEngine;

[CreateAssetMenu(fileName = "RedFaceBasicSettings", menuName = "ScriptableObjects/RedFaceBasicSettings", order = 2)]
public class RedFaceBasicSettings : ScriptableObject
{
    public Material material;

    public float height;
    public float offset;
}
