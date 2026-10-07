using UnityEngine;

namespace Menu.Effects.Flickering
{
    public class FlickeringPresenterScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FlickeringViewScript view;

        [Header("Defaults")]
        [SerializeField] private FlickeringSettings settingsDefault;

        private readonly FlickeringPlanBuilderScript planBuilder = new();

        public void Flicker(Component target, Color initialColor, Color targetColor, float duration, bool isTurningOn, bool isBlinking, bool isDeactivateTargetAfter = false)
        {
            FlickeringRequestScript request = new(target, settingsDefault, initialColor, targetColor, duration, isTurningOn, isBlinking, isDeactivateTargetAfter);
            Flicker(in request);
        }

        public void Flicker(in FlickeringRequestScript request)
        {
            FlickeringPlanScript plan = planBuilder.Build(in request);
            StartCoroutine(view.PlayingFlickeringRoutine(request.Target, plan));
        }
    }
}
