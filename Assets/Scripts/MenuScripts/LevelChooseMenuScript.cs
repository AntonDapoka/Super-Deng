using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System.Collections;

public class LevelChooseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] private Button buttonСhoose;
    [SerializeField] private Button buttonBack;
    [SerializeField] private LevelButtonTransition LBT;

    public Vector3 positionChosen;
    public Vector3 positionUnchosen;
    public RectTransform[] uiElements;
    [SerializeField] private int[] scenes;

    public float moveDuration = 1f; // Время плавного перемещения
    public AnimationCurve movementCurve; // Кривая для движения объектов и UI-элементов

    private void Start()
    {
        buttonСhoose.onClick.AddListener(OnFirstClick);
        buttonBack.onClick.AddListener(LoadMenuScene);
    }

    private void OnFirstClick()
    {
        StartCoroutine(MoveObjectAndUI(true));
        buttonСhoose.onClick.RemoveAllListeners();
        buttonСhoose.onClick.AddListener(LoadCorrespondingScene);

        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(OnSecondClick);
    }

    private void OnSecondClick()
    {
        StartCoroutine(MoveObjectAndUI(false));
        buttonСhoose.onClick.RemoveAllListeners();
        buttonСhoose.onClick.AddListener(OnFirstClick);

        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(LoadMenuScene);
    }

    private IEnumerator MoveObjectAndUI(bool isChosen)
    {
        float elapsedTime = 0f;
        Vector3 initialHolderPos = holder.transform.position;

        Vector3 targetHolderPos = isChosen ? positionChosen : Vector3.zero;

        int positionMultiplier = isChosen ? 1 : -1;

        Vector2[] initialUIPositions = new Vector2[uiElements.Length];
        for (int i = 0; i < uiElements.Length; i++)
        {
            initialUIPositions[i] = uiElements[i].anchoredPosition;
        }

        while (elapsedTime < moveDuration)
        {
            // Используем кривую для определения прогресса движения
            float curveProgress = movementCurve.Evaluate(elapsedTime / moveDuration);

            // Плавное перемещение объекта holder с учетом кривой
            holder.transform.position = Vector3.Lerp(initialHolderPos, targetHolderPos, curveProgress);

            // Плавное перемещение UI-элементов с учетом кривой
            for (int i = 0; i < uiElements.Length; i++)
            {
                uiElements[i].anchoredPosition = Vector2.Lerp(initialUIPositions[i], initialUIPositions[i] - new Vector2(400 * positionMultiplier, 0) , curveProgress);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедимся, что все объекты достигли точной позиции
        holder.transform.position = targetHolderPos;
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].anchoredPosition = initialUIPositions[i] - new Vector2(400, 0) * positionMultiplier;
        }
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
