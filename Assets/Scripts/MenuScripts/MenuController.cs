using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject icosahedron;
    [SerializeField] private MenuLogoNeonFlinkeringScript MLNFS;

    public bool isFlinkeringContinue;
    [Header("Buttons")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonLevel;
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonSettingsBack;

    [Header("UI Elements")]
    [SerializeField] private Image imageSettings;
    [SerializeField] private float biasSettings = 400f;
    [SerializeField] private float moveSettingsDuration = 2f;
    [SerializeField] private AnimationCurve moveSettingsCurve;
    [SerializeField] private Image panel;
    [SerializeField] private Image wall;
    [SerializeField] private float panelFadeDuration;
    [SerializeField] private AnimationCurve panelFadeCurve;

    private void Start()
    {
        buttonStart.onClick.AddListener(OnStartClick);
        buttonLevel.onClick.AddListener(OnLevelClick);
        buttonSettings.onClick.AddListener(OnSettingsClick);
        buttonSettingsBack.onClick.AddListener(OnSettingsBackClick);

        StartCoroutine(PanelFadingInAndOut(false, 0));
    }

    private void OnStartClick()
    {
        StartCoroutine(PanelFadingInAndOut(true, 2));
    }

    private void OnLevelClick()
    {
        Rigidbody[] childRigidbodies = icosahedron.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in childRigidbodies)
        {
            rb.useGravity = true;
        }
    }

    private void OnSettingsClick()
    {
        wall.gameObject.SetActive(true);
        MLNFS.LogoTurningOnAndOff(moveSettingsDuration, false);

        StartCoroutine(MoveObjectAndUI(imageSettings.gameObject, 900f, true, true));
        StartCoroutine(MoveObjectAndUI(buttonStart.gameObject, 300f, false, true));
        StartCoroutine(MoveObjectAndUI(buttonLevel.gameObject, 300f, false, true));
        StartCoroutine(MoveObjectAndUI(buttonSettings.gameObject, 300f, false, true));
    }

    private void OnSettingsBackClick()
    {
        wall.gameObject.SetActive(true);
        StartCoroutine(MoveObjectAndUI(imageSettings.gameObject, 900f, false, false));
        //buttonStart.gameObject.SetActive(true);
        StartCoroutine(MoveObjectAndUI(buttonStart.gameObject, 300f, true, false));
        //buttonLevel.gameObject.SetActive(true);
        StartCoroutine(MoveObjectAndUI(buttonLevel.gameObject, 300f, true, false));
        //buttonSettings.gameObject.SetActive(true);
        StartCoroutine(MoveObjectAndUI(buttonSettings.gameObject, 300f, true, false));

        MLNFS.LogoTurningOnAndOff(moveSettingsDuration, true);
    }


    private IEnumerator MoveObjectAndUI(GameObject obj, float bias, bool isChosen, bool isDown)
    {
        wall.gameObject.SetActive(true);

        if (isChosen)
        {
            obj.SetActive(true);
        }

        float elapsedTime = 0f;

        int positionMultiplier = isDown ? 1 : -1;

        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        Vector2 initialPos = rectTransform.anchoredPosition;

        while (elapsedTime < moveSettingsDuration)
        {
            float curveProgress = moveSettingsCurve.Evaluate(elapsedTime / moveSettingsDuration);

            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, initialPos - new Vector2(0, bias * positionMultiplier), curveProgress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = initialPos - new Vector2(0, bias) * positionMultiplier;
        
        if (!isChosen)
        {
            obj.SetActive(false);
        }

        wall.gameObject.SetActive(false);
    }

    private IEnumerator PanelFadingInAndOut(bool isIn, int indexScene)
    {
        panel.gameObject.SetActive(true);

        Color startColor = panel.color;
        float targetAplha = isIn ? 1f : 0f;

        float elapsedTime = 0f;

        while (elapsedTime < panelFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / panelFadeDuration);
            float curveValue = panelFadeCurve.Evaluate(t);
            float alpha = Mathf.Lerp(startColor.a, targetAplha, curveValue);
            panel.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        panel.color = new Color(startColor.r, startColor.g, startColor.b, targetAplha);

        if (isIn)
        {
            SceneManager.LoadScene(indexScene);
        }
        else
        {
            panel.gameObject.SetActive(false);
        }
    }
}
