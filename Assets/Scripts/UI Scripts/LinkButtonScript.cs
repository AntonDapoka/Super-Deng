using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkButtonScript : MonoBehaviour
{
    [System.Serializable]
    public class ButtonLink
    {
        public Button button;
        public string url;
    }

    [SerializeField] private ButtonLink[] links;

    private void Start()
    {
        foreach (var link in links)
        {
            if (link.button != null && !string.IsNullOrEmpty(link.url))
            {
                string capturedUrl = link.url; // важно захватить в локальную переменную
                link.button.onClick.AddListener(() => Application.OpenURL(capturedUrl));
            }
        }
    }
}