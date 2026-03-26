using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonViewScript : MonoBehaviour, IMenuButtonViewScript
{
    public RectTransform GetRectTransform()
    {
        if (TryGetComponent<RectTransform>(out var rectTransform))
        {
            return rectTransform;
        }
        else
        {
            Debug.LogError("BUTTON WITHOUT TRANSFORM");
            return null;
        }
    }
}
