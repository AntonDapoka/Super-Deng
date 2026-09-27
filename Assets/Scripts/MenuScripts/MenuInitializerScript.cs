using UnityEngine;

public class MenuInitializerScript : MonoBehaviour
{
    [SerializeField] private MenuButtonHolderScript menuButtonHolder;
    [SerializeField] private RectTransform[] buttons;     // REWRITE IT!!!!!!!!!!

    private void Start()
    {
        foreach (var button in buttons)
        {
            if (button.TryGetComponent<IMenuButtonViewScript>(out var buttonView))
            {
                menuButtonHolder.AddButton(buttonView);
            }
        }
    }
}
