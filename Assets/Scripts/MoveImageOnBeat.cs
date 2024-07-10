using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ��� ������ � UI ����������

public class MoveImageOnBeat : MonoBehaviour
{
    public RhythmManager RM;
    public Image uiImage; // ������ �� UI Image

    private RectTransform imageRectTransform;
    private Canvas parentCanvas;

    private float beatInterval; // �������� ������� ����� �������
    private float nextBeatTime; // ����� ���������� �����
    private Vector2 startPosition; // ��������� ������� Image
    private Vector2 centerPosition; // ����������� ������� � Canvas
    private bool isMoving = false; // ���� ��������
    private float moveStartTime; // ����� ������ ��������

    void Start()
    {
        beatInterval = RM.beatInterval;
        nextBeatTime = Time.time + beatInterval;
        imageRectTransform = uiImage.GetComponent<RectTransform>();
        parentCanvas = imageRectTransform.GetComponentInParent<Canvas>();
        startPosition = imageRectTransform.anchoredPosition;
        centerPosition = Vector2.zero; 
        if (imageRectTransform.anchorMin != Vector2.one * 0.5f || imageRectTransform.anchorMax != Vector2.one * 0.5f)
        {
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            centerPosition = new Vector2(canvasRect.rect.width / 2, canvasRect.rect.height / 2);
        }
        this.enabled = false;
    }

    void Update()
    {
        // ���������, �������� �� ������� ����� ���������� ������� �����
        if (Time.time >= nextBeatTime)
        {
            nextBeatTime += beatInterval;
            StartMoveToCenter();
        }
        if (isMoving)
        {
            MoveToCenter();
        }
    }

    void StartMoveToCenter()
    {
        isMoving = true;
        moveStartTime = Time.time;
    }

    void MoveToCenter()
    {
        float elapsed = Time.time - moveStartTime;
        float t = elapsed / beatInterval;

        if (t <= 1f)
        {
            imageRectTransform.anchoredPosition = Vector2.Lerp(startPosition, centerPosition, t);
        }
        else
        {
            imageRectTransform.anchoredPosition = startPosition;
            isMoving = false;
        }
    }
}
