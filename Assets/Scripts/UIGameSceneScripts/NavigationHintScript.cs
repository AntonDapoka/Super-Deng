using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class NavigationHintScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float extraHeight = 1f;
    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;
    [Header("Navigation Hints")]
    [SerializeField] private TextMeshPro textNavigationHintLeft;
    [SerializeField] private TextMeshPro textNavigationHintRight;
    [SerializeField] private TextMeshPro textNavigationHintTop;

    private void Awake()
    {
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));

        textNavigationHintRight.text = keyRight.ToString();
        textNavigationHintLeft.text = keyLeft.ToString();
        textNavigationHintTop.text = keyTop.ToString();
    }

    public void SetNavigationHint(FaceScript FS)
    {
        Transform faceTransform = FS.gameObject.transform;

        TextMeshPro textNavigationHint = null;

        if (FS.isLeft)
            textNavigationHint = textNavigationHintLeft;
        else if (FS.isRight)
            textNavigationHint = textNavigationHintRight;
        else if (FS.isTop)
            textNavigationHint = textNavigationHintTop;

        if (FS.isBlocked)
            textNavigationHint.GetComponent<MeshRenderer>().enabled = false;
        else
            textNavigationHint.GetComponent<MeshRenderer>().enabled = true;

        if (textNavigationHint != null)
        {
            textNavigationHint.transform.SetParent(faceTransform);
            textNavigationHint.transform.localPosition = new Vector3(0, extraHeight, 0);

            textNavigationHint.transform.localRotation = Quaternion.Euler(-90f, 90f, 0);

            TurnToThePlayer(textNavigationHint.transform);
        }
    }

    public void SetNavigationHintTutorial(TutorialFaceScript TFS)
    {
        Transform faceTransform = TFS.gameObject.transform;

        TextMeshPro textNavigationHint = null;

        if (TFS.isLeft)
            textNavigationHint = textNavigationHintLeft;
        else if (TFS.isRight)
            textNavigationHint = textNavigationHintRight;
        else if (TFS.isTop)
            textNavigationHint = textNavigationHintTop;

        if (TFS.isBlocked)
        {
            textNavigationHint.GetComponent<MeshRenderer>().enabled = false;
        }
        else
            textNavigationHint.GetComponent<MeshRenderer>().enabled = true;

        if (textNavigationHint != null)
        {
            textNavigationHint.transform.SetParent(faceTransform);
            textNavigationHint.transform.localPosition = new Vector3(0, extraHeight, 0);
            textNavigationHint.transform.localRotation = Quaternion.Euler(-90f, 90f, 0);
            //textNavigationHint.transform.SetParent(player);
            TurnToThePlayer(textNavigationHint.transform);
        }
    }

    private void TurnToThePlayer(Transform obj)
    {
        Vector3 Look = obj.InverseTransformPoint(player.position);
        float angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg + 90;

        obj.Rotate(0,0,angle);    
    }
}

