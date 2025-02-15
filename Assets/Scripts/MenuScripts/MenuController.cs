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
    [SerializeField] private StartToSavingsTransitionScript STSTS;
    [SerializeField] private MenuCreditsScript MCS;

    public bool isFlinkeringContinue;
    [Header("MainButtons")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonLevel;
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonCredits;
    [Header("OtherButtons")]
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonSettingsSave;
    [SerializeField] private Button buttonSettingsCorrection;
    [SerializeField] private Button buttonSavingsPlay;
    [SerializeField] private Button buttonSavingsDelete;
    [SerializeField] private Button buttonCreditsDonate;
    [SerializeField] private Button buttonCreditsContact;

    [Header("UI Elements")]
    [SerializeField] private Image imageSavings;
    [SerializeField] private Image imageCredits;
    [SerializeField] private Image imageSettings;
    //[SerializeField] private float biasSettings = 400f;
    [SerializeField] private float moveImagesDuration = 2f;
    [SerializeField] private float moveButtonsDuration = 1f;
    [SerializeField] private float waitBetweenButtons = 1f;
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
        buttonCredits.onClick.AddListener(OnCreditsClick);

        StartCoroutine(PanelFadingInAndOut(false, 0));
    }

    private void OnStartClick()
    {
        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(OnSavingsBackClick);

        STSTS.StartTransition();
        /*
        StartCoroutine(SetImageChangeButtons(imageSavings, new[] { buttonStart, buttonLevel, buttonSettings, buttonCredits },
            waitBetweenButtons, new[] { buttonSavingsPlay, buttonSavingsDelete, buttonBack }, false));*/
    }

    private void OnSavingsBackClick()
    {
        StartCoroutine(SetImageChangeButtons(imageSavings, new[] { buttonSavingsPlay, buttonSavingsDelete, buttonBack },
           waitBetweenButtons, new[] { buttonStart, buttonLevel, buttonSettings, buttonCredits }, true, true));
    }

    private void OnLevelClick()
    {
        MLNFS.LogoTurningOnAndOff(moveImagesDuration, 0.1f, 0.4f, false, false);
    }

    private void OnSettingsClick()
    {
        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(OnSettingsBackClick);

        StartCoroutine(SetImageChangeButtons(imageSettings, new[] { buttonStart, buttonLevel, buttonSettings, buttonCredits },
            waitBetweenButtons, new[] { buttonSettingsSave, buttonSettingsCorrection, buttonBack }, false, true));
    }

    private void OnSettingsBackClick()
    {
        StartCoroutine(SetImageChangeButtons(imageSettings, new[] { buttonSettingsSave, buttonSettingsCorrection, buttonBack },
           waitBetweenButtons, new[] { buttonStart, buttonLevel, buttonSettings, buttonCredits }, true, true));

    }

    private void OnCreditsClick()
    {
        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(OnCreditsBackClick);

        MCS.StartCredits();

        StartCoroutine(SetImageChangeButtons(null, new[] { buttonStart, buttonLevel, buttonSettings, buttonCredits },
           waitBetweenButtons, new[] { buttonCreditsContact, buttonCreditsDonate, buttonBack }, false, false));
    }

    private void OnCreditsBackClick()
    {
        StartCoroutine(SetImageChangeButtons(imageCredits, new[] { buttonCreditsContact, buttonCreditsDonate, buttonBack },
           waitBetweenButtons, new[] { buttonStart, buttonLevel, buttonSettings, buttonCredits }, true, false));
    }

    private IEnumerator SetImageChangeButtons(Image image, Button[] buttonsMain, float timeWait, Button[] buttonsExtra, bool isImageUp, bool isInteractWithLogo)
    {
        wall.gameObject.SetActive(true);

        if (isInteractWithLogo) MLNFS.LogoTurningOnAndOff(moveImagesDuration, 0.1f, 0.4f, isImageUp, true);

        if (image != null) 
            StartCoroutine(MoveObjectAndUI(image.gameObject, 900f * (isImageUp ? -1 : 1), moveImagesDuration, true, true));

        foreach (Button button in buttonsMain)
        {
            StartCoroutine(MoveObjectAndUI(button.gameObject, 300f, moveButtonsDuration, false, true));
        }
        yield return new WaitForSeconds(timeWait);

        foreach (Button button in buttonsExtra)
        {
            StartCoroutine(MoveObjectAndUI(button.gameObject, 300f, moveButtonsDuration, true, false));
        }

        float max = System.Math.Max(moveImagesDuration, 2* moveButtonsDuration);

        yield return new WaitForSeconds(max - timeWait);
        wall.gameObject.SetActive(false);
    }

    private IEnumerator MoveObjectAndUI(GameObject obj, float bias, float duration, bool isChosen, bool isDown)
    {

        if (isChosen) obj.SetActive(true);
        
        float elapsedTime = 0f;

        int positionMultiplier = isDown ? 1 : -1;

        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        Vector2 initialPos = rectTransform.anchoredPosition;

        while (elapsedTime < duration)
        {
            float curveProgress = moveSettingsCurve.Evaluate(elapsedTime / duration);

            rectTransform.anchoredPosition = Vector2.Lerp(initialPos, initialPos - new Vector2(0, bias * positionMultiplier), curveProgress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = initialPos - new Vector2(0, bias) * positionMultiplier;
        
        if (!isChosen) obj.SetActive(false);
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

        if (isIn) SceneManager.LoadScene(indexScene);
        else panel.gameObject.SetActive(false);
    }
}
