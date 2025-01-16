using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatFlickeringScript : MonoBehaviour
{
    public Material materialToFade; // Материал, который будет изменяться
    public float fadeDuration = 2f; // Длительность каждой фазы изменения прозрачности
    public float waitDuration = 3f; // Длительность паузы между фазами

    private void Start()
    {
        if (materialToFade == null)
        {
            Debug.LogError("Material to fade is not assigned!");
            return;
        }

        // Убедимся, что материал поддерживает прозрачность
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

        // Сначала устанавливаем материал полностью прозрачным
        SetMaterialAlpha(0f);

        // Плавно увеличиваем прозрачность до 40%
        yield return StartCoroutine(FadeToAlpha(0.4f, fadeDuration));

        // Ждем 3 секунды
        yield return new WaitForSeconds(waitDuration);

        // Плавно увеличиваем прозрачность до 100%
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

        // Убедимся, что финальное значение точно установлено
        SetMaterialAlpha(targetAlpha);
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color color = materialToFade.color;
        color.a = alpha;
        materialToFade.color = color;
    }
}

