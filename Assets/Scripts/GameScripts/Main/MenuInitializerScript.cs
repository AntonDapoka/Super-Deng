using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInitializerScript : MonoBehaviour
{
    // REWRITE IT!!!!!!!!!!
    [SerializeField] private MenuButtonHolderScript menuButtonHolder;
    [SerializeField] private RectTransform[] buttons;


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
