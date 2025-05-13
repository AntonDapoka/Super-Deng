using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordHintScript : MonoBehaviour
{
    public GameObject[] objects;
    public Transform cameraTransform;
    public float moveDuration = 3f;
    public Vector3 offsetFromCamera;

    private void Start()
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        StartCoroutine(ActivateSequence());
    }

    private IEnumerator ActivateSequence()
    {
        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;

            obj.SetActive(true);

            Vector3 targetPosition = offsetFromCamera;
            Vector3 startPosition = obj.transform.position;
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / moveDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            obj.transform.position = targetPosition;

            obj.SetActive(false);
        }
    }
}