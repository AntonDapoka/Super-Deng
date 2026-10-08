using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Effects.Flickering
{
    public class FlickeringViewScript : MonoBehaviour
    {
        public IEnumerator PlayingFlickeringRoutine(Component target, FlickeringPlanScript plan)
        {
            foreach (FlickeringPhaseScript phase in plan.Phases)
            {
                float elapsedTime = 0f;

                while (elapsedTime < phase.Duration)
                {
                    float curveValue = phase.Curve.Evaluate(elapsedTime / phase.Duration);
                    SetColor(target, Color.Lerp(phase.StartColor, plan.TargetColor, curveValue));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            SetColor(target, plan.TargetColor);

            if (plan.DeactivateTargetAfter) target.gameObject.SetActive(false);
        }

        public void SetColorInstantly(Component target, Color color) => SetColor(target, color);

        private static void SetColor(Component target, Color color)
        {
            switch (target)
            {
                case Image image:
                    image.color = color;
                    break;
                case TextMeshProUGUI text:
                    text.color = color;
                    break;
                case Renderer renderer:
                    renderer.material.color = color;
                    break;
                case Button button:
                    button.image.color = color;
                    break;
                default:
                    Debug.LogError($"unsupported color target");
                    break;
            }
        }
    }
}
