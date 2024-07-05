using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurderScript : MonoBehaviour
{
    public GameObject[] cubes;
    public Canvas canvas;
    private int lastCubeIndex = -1;
    public float colorChangeDuration = 1.5f;
    public float scaleChangeDuration = 1f;

    private void Start()
    {
        InvokeRepeating("ChangeCubeColor", 0f, 1f);
    }

    private void ChangeCubeColor()
    {
        int randomIndex = Random.Range(0, cubes.Length);
        while (randomIndex == lastCubeIndex)
        {
            randomIndex = Random.Range(0, cubes.Length);
        }

        if (lastCubeIndex != -1)
        {
            // ���������� ���������� ��� � ��������� ���������
            StartCoroutine(FadeColorAndScale(cubes[lastCubeIndex], Color.white, new Vector3(0.1f, 0.8f, 0.8f), colorChangeDuration, scaleChangeDuration));
        }

        // ���������� ����� ��� � ������� � ����� ����������� ���
        StartCoroutine(ChangeColorThenScale(cubes[randomIndex], Color.red, new Vector3(2f, 0.8f, 0.8f), colorChangeDuration, scaleChangeDuration));
        lastCubeIndex = randomIndex; // ��������� ������ ���������� ����������� ������
    }

    IEnumerator ChangeColorThenScale(GameObject cube, Color targetColor, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        // ������� ������ ����
        yield return StartCoroutine(FadeColor(cube, targetColor, colorDuration));
        if (cube.GetComponent<CubeScript>().havePlayer)
        {
            cube.GetComponent<CubeScript>().player.gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);
        }
        // ����� ������ �������
        yield return StartCoroutine(ChangeScale(cube, targetScale, scaleDuration));
    }

    IEnumerator FadeColor(GameObject cube, Color targetColor, float duration)
    {
        Material material = cube.GetComponent<Renderer>().material;
        Color startColor = material.color;
        float timer = 0f;

        while (timer < duration)
        {
            material.color = Color.Lerp(startColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        material.color = targetColor;
    }

    IEnumerator ChangeScale(GameObject cube, Vector3 targetScale, float duration)
    {
        Vector3 startScale = cube.transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            if (cube.GetComponent<CubeScript>().havePlayer)
            {
                cube.GetComponent<CubeScript>().player.gameObject.SetActive(false);
                canvas.gameObject.SetActive(true);
            }
            cube.transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        cube.transform.localScale = targetScale;
    }

    IEnumerator FadeColorAndScale(GameObject cube, Color targetColor, Vector3 targetScale, float colorDuration, float scaleDuration)
    {
        // ���������������� ��������� ����� � ��������
        yield return StartCoroutine(FadeColor(cube, targetColor, colorDuration));
        yield return StartCoroutine(ChangeScale(cube, targetScale, scaleDuration));
    }
}
