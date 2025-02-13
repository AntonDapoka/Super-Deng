using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System.Collections;

public class LevelChooseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] private Button button�hoose;
    [SerializeField] private Button buttonBack;
    [SerializeField] private LevelButtonTransition LBT;

    public Vector3 positionChosen;
    public Vector3 positionUnchosen;
    public RectTransform[] uiElements;
    [SerializeField] private int[] scenes;

    public float moveDuration = 1f; // ����� �������� �����������
    public AnimationCurve movementCurve; // ������ ��� �������� �������� � UI-���������

    private void Start()
    {
        button�hoose.onClick.AddListener(OnFirstClick);
        buttonBack.onClick.AddListener(LoadMenuScene);
    }

    private void OnFirstClick()
    {
        StartCoroutine(MoveObjectAndUI(true));
        button�hoose.onClick.RemoveAllListeners();
        button�hoose.onClick.AddListener(LoadCorrespondingScene);

        buttonBack.onClick.RemoveAllListeners();
        buttonBack.onClick.AddListener(OnSecondClick);
    }

    private void OnSecondClick()
    {
        StartCoroutine(MoveObjectAndUI(false));
        button�hoose.onClick.RemoveAllListeners();
        button�hoose.onClick.AddListener(OnFirstClick);

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
            // ���������� ������ ��� ����������� ��������� ��������
            float curveProgress = movementCurve.Evaluate(elapsedTime / moveDuration);

            // ������� ����������� ������� holder � ������ ������
            holder.transform.position = Vector3.Lerp(initialHolderPos, targetHolderPos, curveProgress);

            // ������� ����������� UI-��������� � ������ ������
            for (int i = 0; i < uiElements.Length; i++)
            {
                uiElements[i].anchoredPosition = Vector2.Lerp(initialUIPositions[i], initialUIPositions[i] - new Vector2(400 * positionMultiplier, 0) , curveProgress);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������, ��� ��� ������� �������� ������ �������
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
