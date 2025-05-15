using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordHintScript : MonoBehaviour
{
    public GameObject[] objects;
    public float scaleDuration = 3f;
    public Vector3 targetScale = Vector3.one * 2f;

    public AnimationCurve scaleCurve;

    private void Start()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
                objects[i].SetActive(false);
        }

        StartCoroutine(ActivateSequence());
    }

    private IEnumerator ActivateSequence()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject obj = objects[i];
            if (obj == null) continue;

            obj.SetActive(true);

            Vector3 startScale = obj.transform.localScale;
            float elapsed = 0f;

            while (elapsed < scaleDuration)
            {
                float t = elapsed / scaleDuration;
                float curveValue = scaleCurve.Evaluate(t);
                obj.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, curveValue);
                elapsed += Time.deltaTime;
                yield return null;
            }

            obj.transform.localScale = targetScale;
            objects[i].SetActive(true);
            obj.SetActive(false);
            obj.transform.localScale = startScale;
        }
    }
}