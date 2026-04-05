using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartToSavingsTransitionScript : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject[] canvases;
    [SerializeField] private GameObject[] levelObjects;
    [SerializeField] private GameObject prefabDoor;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Transform enviromentHolder;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 doorPosition;
    [SerializeField] private float rotationDuration = 1.5f;
    [SerializeField] private float imageResizeDuration = 0.8f;

    public void StartTransition()
    {
        background.SetActive(false);
        foreach (var levelObject in levelObjects)
            levelObject.SetActive(false);
        foreach (var canvas in canvases)
            canvas.SetActive(false);
        StartCoroutine(PerformEffects());

    }

    private IEnumerator PerformEffects()
    {
        StartCoroutine(RotateCameraToTarget());
        //yield return StartCoroutine(ToggleAndResizeImage(false)); // �������� � �������
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(ExpandDoor()); // ���������� � ����������� �������
        //yield return StartCoroutine(ZoomCamera(originalCameraSize)); // ���������� ���
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ExpandDoor());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ExpandDoor());
    }

    private IEnumerator RotateCameraToTarget()
    {
        float elapsedTime = 0;

        Vector3 startPosition = enviromentHolder.position;
        Vector3 startSize = enviromentHolder.localScale;
        Vector3 targetSize = startSize * 30;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            enviromentHolder.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / rotationDuration);
            enviromentHolder.localScale = Vector3.Lerp(startSize, targetSize, elapsedTime / rotationDuration);
            yield return null;
        }

        enviromentHolder.position = targetPosition;
        enviromentHolder.localScale = targetSize;
    }

    private IEnumerator ExpandDoor()
    { 
        GameObject door = Instantiate(prefabDoor);
        door.transform.position = doorPosition;

        float elapsedTime = 0;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = new Vector3(20f,20f,20f);

        while (elapsedTime < imageResizeDuration)
        {
            elapsedTime += Time.deltaTime;
            door.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / imageResizeDuration);
            yield return null;
        }

        door.transform.localScale = targetScale;

        SceneManager.LoadScene(1);
    }
    /*
    private IEnumerator ToggleAndResizeImage(bool expand)
    {
        uiImage.rectTransform.anchoredPosition = Vector2.zero;
        if (!expand) uiImage.gameObject.SetActive(false);

        float elapsedTime = 0;
        Vector3 startScale = expand ? Vector3.zero : originalImageScale;
        Vector3 targetScale = expand ? originalImageScale : Vector3.zero;

        while (elapsedTime < imageResizeDuration)
        {
            elapsedTime += Time.deltaTime;
            uiImage.rectTransform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / imageResizeDuration);
            yield return null;
        }

        uiImage.rectTransform.localScale = targetScale;

        if (expand) uiImage.gameObject.SetActive(true);
    }*/
}
