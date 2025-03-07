using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogoNeonFlinkeringScript : MonoBehaviour
{
    public bool isTurnOn = true;
    [SerializeField] private GameObject icosahedron;
    [SerializeField] private SparksParticleScript SPS;
    [SerializeField] private Renderer[] renderersGP;
    [SerializeField] private Material materialWhite;
    [SerializeField] private Material materialBlack;
    [SerializeField] private SpriteRenderer triangle;
    [SerializeField] private SpriteRenderer[] logoParts;
    public AnimationCurve colorChangeCurveTurnOn;
    public AnimationCurve colorChangeCurveTurnOff;
    [Header("UI Elements")]
    [SerializeField] private Image wall;
    [Header("Bool Values")]
    [SerializeField] private bool isFlinkeringContinue = false;

    private void Start()
    {
        GlowingPart[] childGlowingParts = icosahedron.GetComponentsInChildren<GlowingPart>();

        renderersGP = new Renderer[childGlowingParts.Length];

        for (int i = 0; i < childGlowingParts.Length; i++)
        {
            renderersGP[i] = childGlowingParts[i].GetComponent<Renderer>();
        }
    }

    public void LogoTurningOnAndOff(float time, float minTimeForTriangle, float maxTimeForTriangle, bool isOn, bool isChangeIcosahedron)
    {
        isTurnOn = isOn;
        //wall.gameObject.SetActive(true);
        if (!isOn) StartCoroutine(FlinkeringOfTriangle(triangle.GetComponent<SpriteRenderer>(), minTimeForTriangle, maxTimeForTriangle, true));
        isFlinkeringContinue = !isOn;
        StartCoroutine(ChangeColorsWithAnimationCurve(time, isOn, isChangeIcosahedron));
        if (!isTurnOn) SPS.StartRandomParticles();
    }


    private IEnumerator ChangeColorsWithAnimationCurve(float time, bool isOn, bool isChangeIcosahedron)
    {
        float elapsedTime = 0f;

        Color initialColor = isOn ? Color.gray : Color.white;
        Color targetColor = isOn ? Color.white : Color.gray;

        if (isChangeIcosahedron) 
        { 
            foreach (var renderer in renderersGP)
            {
                if (renderer != null)
                {
                    StartCoroutine(SettingMaterial(renderer, isOn ? materialWhite : materialBlack, time));
                }
            }
        }
        foreach (var part in logoParts)
        {
            StartCoroutine(ChangeColorSmoothly(part, time, initialColor, targetColor, isOn));
        }
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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

    private IEnumerator SettingMaterial(Renderer renderer, Material material, float time)
    {
        
        yield return new WaitForSeconds(Random.Range(0.3f * time, 0.8f * time));
        renderer.material = material;   
    }

    private IEnumerator FlinkeringOfTriangle(SpriteRenderer triangle, float minTime, float maxTime, bool isWhite)
    {
        float randomTime = Random.Range(minTime, maxTime);

        if (isWhite)
        {
            triangle.color = Color.gray;
            minTime *= 3;
            maxTime *= 3;
        }
        else
        {
            triangle.color = Color.white;
            minTime /= 3;
            maxTime /= 3;
        }

        yield return new WaitForSeconds(randomTime);

        if (isFlinkeringContinue)
        {
            StartCoroutine(FlinkeringOfTriangle(triangle, minTime, maxTime, !isWhite));
        }
        else
        {
            triangle.color = Color.white;
        }
    }

}
