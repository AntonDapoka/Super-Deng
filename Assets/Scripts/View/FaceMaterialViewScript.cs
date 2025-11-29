using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMaterialViewScript : MonoBehaviour, IFaceMaterialViewScript
{
    [Header("Material Mapping")]
    [SerializeField] private Material materialDefault;
    [SerializeField] private Material materialRight;
    [SerializeField] private Material materialLeft;
    [SerializeField] private Material materialTop;
    [SerializeField] private Material materialRedFace;
    [SerializeField] private Material materialRedWave;
    [SerializeField] private Material materialFallFace;
    [SerializeField] private Material materialBonus;
    [SerializeField] private Material materialPortal;

    private Dictionary<MaterialType, Material> _materials;

    private void Awake()
    {
        _materials = new Dictionary<MaterialType, Material>
        {
            {MaterialType.Default, materialDefault},
            {MaterialType.Right, materialRight},
            {MaterialType.Left, materialLeft},
            {MaterialType.Top, materialTop},
            {MaterialType.RedFace, materialRedFace},
            {MaterialType.RedWave, materialRedWave},
            {MaterialType.FallFace, materialFallFace},
            {MaterialType.Bonus, materialBonus},
            {MaterialType.Portal, materialPortal},
        };
    }

    public void SetMaterial(Renderer renderer, MaterialType type)
    {
        renderer.material = _materials[type];
    }
}