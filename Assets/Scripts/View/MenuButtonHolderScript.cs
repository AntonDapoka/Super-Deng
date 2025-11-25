using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonHolderScript : MonoBehaviour
{
    [Header("Настройки расположения")]
    [SerializeField] private RectTransform onCanvasHolder;
    [SerializeField] private float verticalSpacing = 80f;
    [SerializeField] private bool autoRepositionOnStart = true;

    private List<RectTransform> _buttons = new List<RectTransform>();


    public void AddButton(IMenuButtonViewScript buttonView)
    {
        RectTransform rect = buttonView.GetRectTransform();
        _buttons.Add(rect);
        rect.SetParent(onCanvasHolder, false);

        //RepositionButtons();
    }
    

    public void SetButtons(IEnumerable<IMenuButtonViewScript> buttonViews)
    {
        _buttons.Clear();

        foreach (var b in buttonViews)
        {
            RectTransform rect = b.GetRectTransform();
            _buttons.Add(rect);
            rect.SetParent(onCanvasHolder, false);
        }

        RepositionButtons();
    }

    public void RepositionButtons()
    {
        if (_buttons.Count == 0)
            return;

        float totalHeight = (_buttons.Count - 1) * verticalSpacing;

        float startY = totalHeight * 0.5f;
        // Первая кнопка — сверху, последняя — снизу, центрируется автоматически

        for (int i = 0; i < _buttons.Count; i++)
        {
            float y = startY - i * verticalSpacing;
            RectTransform rect = _buttons[i];

            rect.anchoredPosition = new Vector2(0f, y);
        }
    }


    private void Start()
    {
        if (autoRepositionOnStart)
            RepositionButtons();
    }
}
