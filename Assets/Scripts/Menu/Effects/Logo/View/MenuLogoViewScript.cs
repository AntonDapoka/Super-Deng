using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu.Effects.Flickering.Logo
{
    public class MenuLogoViewScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject levelIcon;
        [SerializeField] private SpriteRenderer logoTriangle;
        [SerializeField] private SpriteRenderer[] logoParts;
        [SerializeField] private SparksParticleScript sparksManager;

        private Renderer[] levelIconGlowingRenderers;
        private bool isTriangleFlickering;

        public IReadOnlyList<SpriteRenderer> LogoParts => logoParts;

        private void Awake()
        {
            GlowingPart[] glowingParts = levelIcon.GetComponentsInChildren<GlowingPart>();
            levelIconGlowingRenderers = new Renderer[glowingParts.Length];

            for (int i = 0; i < glowingParts.Length; i++)
                levelIconGlowingRenderers[i] = glowingParts[i].GetComponent<Renderer>();
        }

        public void SwapLevelIconMaterial(Material material, float duration, float minDelayFactor)
        {
            foreach (Renderer glowingRenderer in levelIconGlowingRenderers)
                if (duration <= 0f) glowingRenderer.material = material;
                else StartCoroutine(SwappingLevelIconMaterial(glowingRenderer, material, duration, minDelayFactor));
        }

        public void SetTriangleColor(Color color)
        {
            StopTriangleFlicker();
            logoTriangle.color = color;
        }

        public void StartTriangleFlicker(Color colorPrimary, Color colorSecondary, Vector2 intervalRange, float factorPrimary, float factorSecondary)
        {
            if (isTriangleFlickering) return;

            isTriangleFlickering = true;
            StartCoroutine(TriangleFlickeringRoutine(colorPrimary, colorSecondary, intervalRange, factorPrimary, factorSecondary, isShowPrimaryColor: false));
        }

        public void StopTriangleFlicker() => isTriangleFlickering = false;

        public void PlaySparks() => sparksManager.StartRandomParticles();

        private IEnumerator SwappingLevelIconMaterial(Renderer glowingRenderer, Material material, float duration, float minDelayFactor)
        {
            float delay = Random.Range(duration * minDelayFactor, duration);
            yield return new WaitForSeconds(delay);
            glowingRenderer.material = material;
        }

        private IEnumerator TriangleFlickeringRoutine(Color primaryColor, Color secondaryColor, Vector2 intervalRange, float factorPrimary, float factorSecondary, bool isShowPrimaryColor)
        {
            Vector2 currentIntervalRange = intervalRange;

            while (isTriangleFlickering)
            {
                logoTriangle.color = isShowPrimaryColor ? primaryColor : secondaryColor;

                float waitTime = Random.Range(currentIntervalRange.x, currentIntervalRange.y);
                
                yield return new WaitForSeconds(waitTime);
                currentIntervalRange = intervalRange;

                isShowPrimaryColor = !isShowPrimaryColor;

                float intervalFactor = isShowPrimaryColor ? factorPrimary : factorSecondary;
                currentIntervalRange *= intervalFactor;
                
            }

            logoTriangle.color = primaryColor;
        }
    }
}
