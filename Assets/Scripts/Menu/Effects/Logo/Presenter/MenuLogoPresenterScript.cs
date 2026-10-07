using UnityEngine;

namespace Menu.Effects.Flickering.Logo
{
    public class MenuLogoPresenterScript : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FlickeringPresenterScript flickeringPresenter;
        [SerializeField] private MenuLogoViewScript view;
        [SerializeField] private MenuLogoSettings settings;

        public bool IsTurnOn { get; private set; }

        public void TurnOn(bool isFlickerTriangle = false, float? duration = null)
        {
            SetLogoState(
                isTurningOn: true,
                duration: duration ?? settings.DurationChangeBasic,
                isDisappearMode: false,
                isFlickerTriangle
            );
        }

        public void TurnOff( bool isDisappearMode = false, bool isFlickerTriangle = false, float? duration = null)
        {
            SetLogoState(
                isTurningOn: false, 
                duration: duration ?? settings.DurationChangeBasic, 
                isDisappearMode, 
                isFlickerTriangle);
        }

        public void EnsureTurnOn()
        {
            if (!IsTurnOn) TurnOn();
        }

        private void SetLogoState(bool isTurningOn, float duration, bool isDisappearMode, bool isFlickerTriangle)
        {
            IsTurnOn = isTurningOn;
            Color colorInitial = isTurningOn ? GetOffColor(isDisappearMode) : settings.ColorLogoOn;
            Color colorTarget = isTurningOn ? settings.ColorLogoOn : GetOffColor(isDisappearMode);

            Material materialLevelIcon = isTurningOn
                ? settings.MaterialIcosahedronOn
                : settings.MaterialIcosahedronOff;
            view.SwapLevelIconMaterial(materialLevelIcon, duration, settings.FactorMaterialSwapDelayMin);

            foreach (SpriteRenderer logoPart in view.LogoParts)
            {
                FlickeringRequestScript request = new(logoPart, settings.FlickeringSettings, colorInitial, colorTarget, duration, isTurningOn, isBlinking: true);
                flickeringPresenter.Flicker(in request);
            }

            if (isFlickerTriangle && IsAbleToFlickerTriangle(duration))
                view.StartTriangleFlicker(
                settings.ColorTriangleOn, 
                settings.ColorTriangleOff, 
                settings.IntervalRangeTriangleFlicker, 
                settings.FactorTriangleFlickerPrimary, 
                settings.FactorTriangleFlickerSecondary);
            else
                view.SetTriangleColor(colorTarget);

            if (isTurningOn && settings.IsPlaySparksOnTurnOn) view.PlaySparks();
        }

        private bool IsAbleToFlickerTriangle(float duration)
        {
            return duration > 0f && settings.IntervalRangeTriangleFlicker.x > 0f;
        }

        private Color GetOffColor(bool isDisappearMode)
        {
            return isDisappearMode ? settings.ColorLogoHidden : settings.ColorLogoOff;
        }
    }
}
