using UnityEngine;

[CreateAssetMenu(fileName = "FallFaceBasicSettings", menuName = "ScriptableObjects/FallFaceBasicSettings", order = 6)]
public class FallFaceBasicSettings : ScriptableObject
{
    public Material material;

    public float height;
    public float offset;
}
