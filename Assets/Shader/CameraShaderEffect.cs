
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraShaderEffect : MonoBehaviour
{
    [SerializeField] private Shader effectShader;

    private Material effectMaterial;

    private void OnEnable()
    {
        CreateMaterial();
    }

    private void CreateMaterial()
    {
        if (effectShader == null)
            return;

        if (effectMaterial != null)
            DestroyImmediate(effectMaterial);

        effectMaterial = new Material(effectShader);
        effectMaterial.hideFlags = HideFlags.HideAndDontSave;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (effectShader == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        if (effectMaterial == null)
            CreateMaterial();

        if (effectMaterial == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        Graphics.Blit(source, destination, effectMaterial);
    }

    private void OnDisable()
    {
        if (effectMaterial != null)
        {
            DestroyImmediate(effectMaterial);
            effectMaterial = null;
        }
    }
}
