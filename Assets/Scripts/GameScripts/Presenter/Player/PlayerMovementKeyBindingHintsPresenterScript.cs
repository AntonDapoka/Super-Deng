using UnityEngine;
using TMPro;

public class PlayerMovementKeyBindingHintsPresenterScript : MonoBehaviour
{
    [SerializeField] private bool isTurnOn;
    [SerializeField] private KeyBindingDataScript keyBindingData;
    [SerializeField] private float extraHeight = 0.1f;

    private KeyCode keyLeft = KeyCode.A;
    private KeyCode keyTop = KeyCode.W;
    private KeyCode keyRight = KeyCode.D;

    [Header("Navigation Hints")]
    [SerializeField] private TextMeshPro textNavigationHintLeft;
    [SerializeField] private TextMeshPro textNavigationHintRight;
    [SerializeField] private TextMeshPro textNavigationHintTop;
    public bool isUpsideDown = false;

   public void Initialize()
    {
        if (isTurnOn)
        {
            textNavigationHintRight.gameObject.SetActive(true);
            textNavigationHintLeft.gameObject.SetActive(true);
            textNavigationHintTop.gameObject.SetActive(true);
        }
    }

    public void SetKeyBindings(KeyBindingDataScript keyBindingDataNew)
    {
        keyBindingData = keyBindingDataNew;

        keyLeft = keyBindingData.moveLeft;
        keyTop = keyBindingData.moveTop;
        keyRight = keyBindingData.moveRight;

        textNavigationHintRight.text = keyRight.ToString();
        textNavigationHintLeft.text = keyLeft.ToString();
        textNavigationHintTop.text = keyTop.ToString();
    }

    public void SetNavigationHint(Transform playerTransform, FaceScript faceScript)
    {
        if (!isTurnOn) return;

        FaceStateScript faceState = faceScript.FaceState;
        Transform faceTransform = faceScript.gameObject.transform;

        TextMeshPro textNavigationHint = null;

        if (faceState.GetFaceState(FaceProperty.IsLeft))
            textNavigationHint = textNavigationHintLeft;
        else if (faceState.GetFaceState(FaceProperty.IsRight))
            textNavigationHint = textNavigationHintRight;
        else if (faceState.GetFaceState(FaceProperty.IsTop))
            textNavigationHint = textNavigationHintTop;

        if (textNavigationHint != null)
        {
            MeshRenderer renderer = textNavigationHint.GetComponent<MeshRenderer>();

            if (faceState.GetFaceState(FaceProperty.IsBlocked))
                renderer.enabled = false;
            else
                renderer.enabled = true;

            Transform navigationHintTransform = textNavigationHint.transform;

            navigationHintTransform.SetParent(faceTransform);
            navigationHintTransform.localPosition = new(0, extraHeight, 0);
            navigationHintTransform.localRotation = Quaternion.Euler(-90f, 90f, 0);

            TurnToThePlayer(playerTransform, navigationHintTransform);
        }
    }

    private void TurnToThePlayer(Transform playerTransform, Transform obj)
    {
        Vector3 Look = obj.InverseTransformPoint(playerTransform.position);
        float angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg + 90;

        obj.Rotate(0,0,angle);    
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }
}

