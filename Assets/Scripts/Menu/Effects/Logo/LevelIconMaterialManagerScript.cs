using System.Collections;
using UnityEngine;

namespace Menu.Effects.Flickering.Logo
{
    public class LevelIconMaterialManagerScript : MonoBehaviour
    {
        public void SwapMaterial(GameObject levelIcon, Material material, float duration, float minDelayFactor)
        {
            Renderer[] glowingRenderers = GetRenderersFromLevelIcon(levelIcon);
            if (glowingRenderers == null) return;

            foreach (Renderer glowingRenderer in glowingRenderers)
                if (duration <= 0f) glowingRenderer.material = material;
                else StartCoroutine(SwappingMaterial(glowingRenderer, material, duration, minDelayFactor));
        }

        private Renderer[] GetRenderersFromLevelIcon(GameObject levelIcon)
        {
            GlowingPart[] glowingParts = levelIcon.GetComponentsInChildren<GlowingPart>();
            Renderer[] glowingRenderers = new Renderer[glowingParts.Length];

            for (int i = 0; i < glowingParts.Length; i++)
                glowingRenderers[i] = glowingParts[i].GetComponent<Renderer>();

            return glowingRenderers;
        }

        private IEnumerator SwappingMaterial(Renderer glowingRenderer, Material material, float duration, float minDelayFactor)
        {
            float delay = Random.Range(duration * minDelayFactor, duration);
            yield return new WaitForSeconds(delay);
            glowingRenderer.material = material;
        }
    }
}
