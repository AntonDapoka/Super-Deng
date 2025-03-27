using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogoNeonFlinkeringScript : MonoBehaviour
{
    public bool isTurnOn = true;
    [Header("Main Objects")]
    [SerializeField] private GameObject icosahedron;
    [SerializeField] private Renderer[] renderersGlowingParts;
    [SerializeField] private SparksParticleScript SPS;
    [Header("Materials")]
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialBlack;
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer triangle;
    [SerializeField] private SpriteRenderer[] logoParts;
    [Header("Animation Curves")]
    public AnimationCurve colorChangeCurveTurnOn;
    public AnimationCurve colorChangeCurveTurnOff;
    [Header("Bool Values")]
    [SerializeField] private bool isFlinkeringContinue = false;

    private void Start()
    {
        GlowingPart[] childGlowingParts = icosahedron.GetComponentsInChildren<GlowingPart>();
        renderersGlowingParts = new Renderer[childGlowingParts.Length];
        for (int i = 0; i < childGlowingParts.Length; i++)
            renderersGlowingParts[i] = childGlowingParts[i].GetComponent<Renderer>();
    }

    public void LogoTurningOnAndOff(float time, bool TurnOn, bool isChangeIcosahedron, bool isSetParticles, bool isFlickeringTriangle, float minTimeForTriangle = 0f, float maxTimeForTriangle = 0f)
    {
        isTurnOn = TurnOn;

        ChangeColors(time, TurnOn, isChangeIcosahedron, isFlickeringTriangle);

        if (isFlickeringTriangle && maxTimeForTriangle != 0f && time != 0f)
        {
            StartCoroutine(FlinkeringOfTriangle(triangle, minTimeForTriangle, maxTimeForTriangle, true));
            isFlinkeringContinue = true;
        }
        else isFlinkeringContinue = false;

        if (isSetParticles) SPS.StartRandomParticles();
    }

    private void ChangeColors(float time, bool isOn, bool isChangeIcosahedron, bool isFlickeringTriangle)
    {
        Color initialColor = isOn ? Color.gray : Color.white;
        Color targetColor = isOn ? Color.white : Color.gray;

        if (isChangeIcosahedron) 
        { 
            foreach (var renderer in renderersGlowingParts)
            {
                if (time != 0)
                    StartCoroutine(SettingMaterial(renderer, isOn ? materialWhite : materialBlack, time));
                else
                {
                    renderer.material = isOn ? materialWhite : materialBlack;
                }
            }
        }
        foreach (var part in logoParts)
        {
            if (time != 0)
                StartCoroutine(ChangeColorSmoothly(part, time, initialColor, targetColor, isOn));
            else
            {
                part.material.color = targetColor;
            }
        }

        if (!isFlickeringTriangle)
        {
            triangle.material.color = targetColor;
        }
    }

    private IEnumerator SettingMaterial(Renderer renderer, Material material, float time)
    {
        yield return new WaitForSeconds(Random.Range(0.25f * time, time)); //0.3f & 0.8f
        renderer.material = material;
    }

    private IEnumerator ChangeColorSmoothly(Renderer renderer, float time, Color initialColor, Color targetColor, bool isTurnOn)
    {
        float elapsedTime = 0f;
        float randomTime = 0f;
        int interruptionCount = Random.Range(0, 2);
        
        Color initialColorSafer = initialColor;

        if (interruptionCount == 1)
        {
            randomTime = isTurnOn ? Random.Range(time * 0.4f, time * 0.8f) : Random.Range(time * 0.2f, time * 0.6f);

            while (elapsedTime < randomTime)
            {
                float curveValue = isTurnOn ? colorChangeCurveTurnOn.Evaluate(elapsedTime / randomTime) : colorChangeCurveTurnOff.Evaluate(elapsedTime / randomTime);
                Color newColor = Color.Lerp(initialColor, targetColor, curveValue);
                renderer.material.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
        } 
        
        while (elapsedTime < (time - randomTime))
        {
            float curveValue = isTurnOn ? colorChangeCurveTurnOn.Evaluate(elapsedTime / (time - randomTime)) : colorChangeCurveTurnOff.Evaluate(elapsedTime / (time - randomTime));
            Color newColor = Color.Lerp(initialColorSafer, targetColor, curveValue);
            renderer.material.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color = targetColor;
    }

    private IEnumerator FlinkeringOfTriangle(SpriteRenderer triangle, float minTime, float maxTime, bool isWhite)
    {
        
        float randomTime = Random.Range(minTime, maxTime);
        Debug.Log(randomTime);
        triangle.color = isWhite ? Color.gray : Color.white;
        minTime = isWhite ? minTime * 5 : minTime / 5;
        maxTime = isWhite ? maxTime * 5 : maxTime / 5;

        yield return new WaitForSeconds(randomTime);

        if (isFlinkeringContinue)
            StartCoroutine(FlinkeringOfTriangle(triangle, minTime, maxTime, !isWhite));
        else
            triangle.color = Color.white;
    }

}
