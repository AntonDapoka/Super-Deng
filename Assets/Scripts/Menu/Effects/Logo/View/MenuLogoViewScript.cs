using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu.Effects.Flickering.Logo
{
    public class MenuLogoViewScript : MonoBehaviour
    {
        [Header("References")]
        private GameObject levelIcon;
        [SerializeField] private SpriteRenderer logoTriangle;
        [SerializeField] private SpriteRenderer[] logoParts;
        [SerializeField] private SparksParticleScript sparksManager;
        [SerializeField] private LevelIconMaterialManagerScript levelIconMaterialManager;

        private bool isTriangleFlickering;

        public IReadOnlyList<SpriteRenderer> LogoParts => logoParts;

        public void SwapLevelIconMaterial(Material material, float duration, float minDelayFactor)
        {
            levelIconMaterialManager.SwapMaterial(levelIcon, material, duration, minDelayFactor);
        }

        public void SetLevelIcon(GameObject levelIcon)
        {
            this.levelIcon = levelIcon;
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
