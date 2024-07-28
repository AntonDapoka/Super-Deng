using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimationScript : MonoBehaviour
{
    public GameObject[] objectsToAnimate;
    public AnimationClip animationClip;
    public float moveDuration = 1.0f; // �����, �� ������� ������� ����� �����������

    private void Start()
    {
        foreach (GameObject obj in objectsToAnimate)
        {
            obj.SetActive(false);
            // ������������� ��������� ������� ��������
            obj.transform.position = new Vector3(obj.transform.position.x, -4, obj.transform.position.z);
        }
        StartCoroutine(PlayAnimationsWithDelay());
    }

    private IEnumerator PlayAnimationsWithDelay()
    {
        foreach (GameObject obj in objectsToAnimate)
        {
            Animation anim = obj.GetComponent<Animation>();
            if (anim != null)
            {
                obj.SetActive(true);
                anim.AddClip(animationClip, animationClip.name);

                // ��������� �������� ��� �������� �������
                StartCoroutine(MoveObjectUp(obj));

                anim.Play(animationClip.name);

                // ���� 1 ������� ����� �������� ��������� ��������
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator MoveObjectUp(GameObject obj)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, 0, startPosition.z);

        while (elapsedTime < moveDuration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���������, ��� ������� ����������� ����� � �������� ��������
        obj.transform.position = endPosition;
    }
}