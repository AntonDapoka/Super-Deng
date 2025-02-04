using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimationScript : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToAnimate;
    [SerializeField] private AnimationClip animationClip1;
    [SerializeField] private AnimationClip animationClip2;
    [SerializeField] private TutorialManager TM;
    [SerializeField] private float moveDuration = 1.0f; // �����, �� ������� ������� ����� �����������

    private void Start()
    {
        foreach (GameObject obj in objectsToAnimate)
        {
            obj.SetActive(false);
        }
    }

    public void StartAnimation()
    {
        foreach (GameObject obj in objectsToAnimate)
        {
            // ������������� ��������� ������� ��������
            obj.transform.position = new Vector3(obj.transform.position.x, -4, obj.transform.position.z);
        }
        StartCoroutine(PlayAnimationsWithDelay());
    }

    private IEnumerator PlayAnimationsWithDelay()
    {
        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            GameObject obj = objectsToAnimate[i];
            Animation anim = obj.GetComponent<Animation>();

            if (anim != null)
            {
                obj.SetActive(true);
                AnimationClip clipToPlay = (i % 2 == 0) ? animationClip1 : animationClip2;
                anim.AddClip(clipToPlay, clipToPlay.name);
                StartCoroutine(MoveObjectUp(obj));
                anim.Play(clipToPlay.name);
                yield return new WaitForSeconds(0.1f);
            }
        }
        //TM.isAnimationEnded = true;
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