using UnityEngine;

[CreateAssetMenu(fileName = "PortalBasicSettings", menuName = "ScriptableObjects/PortalBasicSettings", order = 10)]
public class PortalBasicSettings : ScriptableObject
{
    public Material material;

    public float height;
    public float offset;
}
