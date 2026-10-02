using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOutScript : MonoBehaviour
{
    [SerializeField] private Image panelFade;
    [SerializeField] private float durationFadeIn;
    [SerializeField] private float durationFadeOut;
    [SerializeField] private bool isStartFadeIn;

    private void Awake()
    {
        if (panelFade != null)
        {
            Color c = panelFade.color;
            c.a = 0f;
            panelFade.color = c;
        }
    }

    private void Start()
    {
        if (isStartFadeIn)
            StartCoroutine(PlayFadeIn());
    }

    public IEnumerator PlayFadeOut()
    {
        panelFade.gameObject.SetActive(true);

        if (panelFade == null)
        {
            Debug.LogWarning("fadeImage не назначен!");
            yield break;
        }

        float elapsed = 0f;

        Color color = panelFade.color;
        color.a = 0f;
        panelFade.color = color;

        while (elapsed < durationFadeOut)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / durationFadeOut);
            color.a = alpha;
            panelFade.color = color;
            yield return null;
        }

        color.a = 1f;
        panelFade.color = color;


    }

    private IEnumerator PlayFadeIn()
    {
        panelFade.gameObject.SetActive(true);

        if (panelFade == null)
        {
            Debug.LogWarning("fadeImage не назначен!");
            yield break;
        }

        float elapsed = 0f;

        Color color = panelFade.color;
        color.a = 1f;
        panelFade.color = color;

        while (elapsed < durationFadeIn)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / durationFadeIn);
            color.a = alpha;
            panelFade.color = color;
            yield return null;
        }

        color.a = 0f;
        panelFade.color = color;

        panelFade.gameObject.SetActive(false);
    }

}
