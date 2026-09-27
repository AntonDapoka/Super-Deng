using System.Collections.Generic;
using UnityEngine;

public class MenuButtonHolderScript : MonoBehaviour
{
    [SerializeField] private RectTransform holder;
    [SerializeField] private float spacingHorizontal = 80f;
    [SerializeField] private bool isRepositionedOnStart = true;

    private List<RectTransform> buttons = new();

    private void Start()
    {
        if (isRepositionedOnStart) RepositionButtons();
    }

    public void AddButton(IMenuButtonViewScript buttonView)
    {
        RectTransform rect = buttonView.GetRectTransform();
        buttons.Add(rect);
        rect.SetParent(holder, false);

        RepositionButtons();
    }

    public void SetButtons(IEnumerable<IMenuButtonViewScript> buttonViews)
    {
        buttons.Clear();

        foreach (var b in buttonViews)
        {
            RectTransform rect = b.GetRectTransform();
            buttons.Add(rect);
            rect.SetParent(holder, false);
        }

        RepositionButtons();
    }

    public void RepositionButtons()
    {
        if (buttons.Count == 0)
            return;

        float totalHeight = (buttons.Count - 1) * spacingHorizontal;

        float startY = totalHeight * 0.5f;

        for (int i = 0; i < buttons.Count; i++)
        {
            float y = startY - i * spacingHorizontal;
            RectTransform rect = buttons[i];

            rect.anchoredPosition = new Vector2(0f, y);
        }
    }
}
