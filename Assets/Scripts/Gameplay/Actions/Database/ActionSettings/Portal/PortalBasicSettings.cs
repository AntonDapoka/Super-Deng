using UnityEngine;

[CreateAssetMenu(fileName = "PortalBasicSettings", menuName = "ScriptableObjects/PortalBasicSettings", order = 10)]
public class PortalBasicSettings : ActionBasicSettingsScript
{
    public Material material;

    public float height;
    public float offset;
}
