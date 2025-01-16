using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatFlickeringScript : MonoBehaviour
{
    public Material materialToFade; // ��������, ������� ����� ����������
    public float fadeDuration = 2f; // ������������ ������ ���� ��������� ������������
    public float waitDuration = 3f; // ������������ ����� ����� ������

    private void Start()
    {
        if (materialToFade == null)
        {
            Debug.LogError("Material to fade is not assigned!");
            return;
        }

        // ��������, ��� �������� ������������ ������������
        if (materialToFade.HasProperty("_Color"))
        {
            StartCoroutine(FadeMaterial());
        }
        else
        {
            Debug.LogError("The assigned material does not have a '_Color' property.");
        }
    }

    private IEnumerator FadeMaterial()
    {
        Color originalColor = materialToFade.color;

        // ������� ������������� �������� ��������� ����������
        SetMaterialAlpha(0f);

        // ������ ����������� ������������ �� 40%
        yield return StartCoroutine(FadeToAlpha(0.4f, fadeDuration));

        // ���� 3 �������
        yield return new WaitForSeconds(waitDuration);

        // ������ ����������� ������������ �� 100%
        yield return StartCoroutine(FadeToAlpha(1f, fadeDuration));
    }

    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        Color color = materialToFade.color;
        float startAlpha = color.a;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            SetMaterialAlpha(newAlpha);
            yield return null;
        }

        // ��������, ��� ��������� �������� ����� �����������
        SetMaterialAlpha(targetAlpha);
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color color = materialToFade.color;
        color.a = alpha;
        materialToFade.color = color;
    }
}

