using UnityEngine;

[CreateAssetMenu(fileName = "RedFaceBasicSettings", menuName = "ScriptableObjects/RedFaceBasicSettings", order = 3)]
public class RedFaceBasicSettings : ScriptableObject
{
    public Material material;

    public float height;
    public float offset;
}
