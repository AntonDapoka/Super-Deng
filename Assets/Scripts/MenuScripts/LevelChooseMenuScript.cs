using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System.Collections;

public class LevelChooseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] private Button button—hoose;
    [SerializeField] private TextMeshProUGUI textButton—hoose;
    [SerializeField] private Button buttonBack;
    [SerializeField] private LevelButtonTransition LBT;

    [SerializeField] private Vector3 positionChosen;
    [SerializeField] private Vector3 positionUnchosen;
    [SerializeField] private RectTransform bannerLevelDesc;
    [SerializeField] private RectTransform[] uiElements;
    [SerializeField] private GameObject wall;
    [SerializeField] private int[] scenes;

    public float biasBanner = 700f;
    public float biasUI = 400f;
    public float moveDuration = 1f; 
    public AnimationCurve movementCurve;

    private void Start()
    {
        textButton—hoose.text = "Choose";
        button—hoose.onClick.AddListener(OnFirstClick);
        buttonBack.onClick.AddListener(LoadMenuScene);
        positionUnchosen = Vector3.zero;
        wall.SetActive(false);
    }

    private void OnFirstClick()
    {
        StartCoroutine(MoveObjectAndUI(true));
        button—hoose.onClick.RemoveAllListeners();
        button—hoose.onClick.AddListener(LoadCorrespondingScene);

        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(OnSecondClick);

        textButton—hoose.text = "Start";
    }

    private void OnSecondClick()
    {
        StartCoroutine(MoveObjectAndUI(false));
        button—hoose.onClick.RemoveAllListeners();
        button—hoose.onClick.AddListener(OnFirstClick);

        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(LoadMenuScene);

        textButton—hoose.text = "Choose";
    }

    private IEnumerator MoveObjectAndUI(bool isChosen)
    {
        wall.SetActive(true);

        float elapsedTime = 0f;
        Vector3 initialHolderPos = holder.transform.position;
        Vector3 targetHolderPos = isChosen ? positionChosen : positionUnchosen;

        int positionMultiplier = isChosen ? 1 : -1;

        Vector2[] initialUIPositions = new Vector2[uiElements.Length];
        for (int i = 0; i < uiElements.Length; i++)
        {
            initialUIPositions[i] = uiElements[i].anchoredPosition;
        }

        Vector2 initialBannerPos = bannerLevelDesc.anchoredPosition;

        while (elapsedTime < moveDuration)
        {
            float curveProgress = movementCurve.Evaluate(elapsedTime / moveDuration);
            holder.transform.position = Vector3.Lerp(initialHolderPos, targetHolderPos, curveProgress);
            for (int i = 0; i < uiElements.Length; i++)
            {
                uiElements[i].anchoredPosition = Vector2.Lerp(initialUIPositions[i], initialUIPositions[i] - new Vector2(biasUI * positionMultiplier, 0) , curveProgress);
            }
            bannerLevelDesc.anchoredPosition = Vector2.Lerp(initialBannerPos, initialBannerPos - new Vector2(biasBanner * positionMultiplier, 0), curveProgress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        holder.transform.position = targetHolderPos;
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].anchoredPosition = initialUIPositions[i] - new Vector2(biasUI, 0) * positionMultiplier;
        }
        bannerLevelDesc.anchoredPosition = initialBannerPos - new Vector2(biasBanner, 0) * positionMultiplier;

        wall.SetActive(false);
    }

    private void LoadCorrespondingScene()
    {
        SceneManager.LoadScene(scenes[LBT._numberMenu]);
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
