using UnityEngine;
using UnityEngine.UI;

public class VarySoundButtonScript : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private AudioClip clipButton;
    [SerializeField] private VaryPitchSoundScript VPSS;

    private void Start()
    {
        foreach (var button in buttons)
        {
            if (button != null && clipButton != null)
            {
                button.onClick.AddListener(() => VPSS.PlayVaryPitchSound(clipButton));
            }
        }
    }
}
