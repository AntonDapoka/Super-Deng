using UnityEngine;

namespace Menu.Effects.Flickering.Logo
{
    [CreateAssetMenu(fileName = "MenuLogoSettings", menuName = "Menu/Effects/Menu Logo Settings")]
    public class MenuLogoSettings : ScriptableObject
    {
        [SerializeField] private float durationChangeBasic = 0.4f;

        [Header("Flickering")]
        [SerializeField] private FlickeringSettings flickeringSettings;

        [Header("Level Icon Materials")]
        [SerializeField] private Material materialLevelIconOn;
        [SerializeField] private Material materialLevelIconOff;
        [SerializeField, Range(0f, 1f)] 
        private float factorMaterialSwapDelayMin = 0.25f;  
        //Minimal material swap delay as a fraction of the transition duration

        [Header("Logo Colors")]
        [SerializeField] private Color colorLogoOn = Color.white;
        [SerializeField] private Color colorLogoOff = Color.gray;
        [SerializeField] private Color colorLogoHidden = Color.clear;
        [SerializeField] private Color colorTriangleOn = Color.white;
        [SerializeField] private Color colorTriangleOff = Color.gray;

        [Header("Triangle Flicker")]
        [SerializeField] private Vector2 intervalRangeTriangleFlicker = new(0.05f, 0.2f); //Base interval range between triangle color toggles, in seconds.
        [SerializeField, Min(0.01f)]
        private float factorTriangleFlickerPrimary = 5f; 
        //Interval multiplier applied when the triangle switches to its primary color
        [SerializeField, Min(0.01f)]
        private float factorTriangleFlickerSecondary = 1f; 
        //Interval divisor applied when the triangle switches back from its primary color."

        [Header("Sparks")]
        [SerializeField] private bool isPlaySparksOnTurnOn = true;

        public float DurationChangeBasic => durationChangeBasic;
        public FlickeringSettings FlickeringSettings => flickeringSettings;
        public Material MaterialIcosahedronOn => materialLevelIconOn;
        public Material MaterialIcosahedronOff => materialLevelIconOff;
        public float FactorMaterialSwapDelayMin => factorMaterialSwapDelayMin;
        public Color ColorLogoOn => colorLogoOn;
        public Color ColorLogoOff => colorLogoOff;
        public Color ColorLogoHidden => colorLogoHidden;
        public Color ColorTriangleOn => colorTriangleOn;
        public Color ColorTriangleOff => colorTriangleOff;
        public Vector2 IntervalRangeTriangleFlicker => intervalRangeTriangleFlicker;
        public float FactorTriangleFlickerPrimary => factorTriangleFlickerPrimary;
        public float FactorTriangleFlickerSecondary => factorTriangleFlickerSecondary;
        public bool IsPlaySparksOnTurnOn => isPlaySparksOnTurnOn;
    }
}
