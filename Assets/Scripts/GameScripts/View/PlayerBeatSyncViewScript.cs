using UnityEngine;

public class PlayerBeatSyncViewScript : MonoBehaviour
{
    public void SetMaterialAlpha(Material material, float alpha)
    {
        Color color = material.color;
        color.a = alpha;
        material.color = color;
    }
}
