using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerMovementKeyBindingHintsPresenterScript : MonoBehaviour
{
    [SerializeField] private bool isTurnOn;
    [SerializeField] private MovementKeyBindingDataScript keyBindingData;
    [SerializeField] private float extraHeight = 0.1f;
    public bool isUpsideDown = false;

    private TextMeshPro[] navigationHints;
    private readonly Dictionary<FaceProperty, TextMeshPro> directionToHint = new();

    public void Initialize(GameObject[] navigationHintsObjects)
    {
        if (navigationHintsObjects == null || navigationHintsObjects.Length == 0)
        {
            Debug.LogWarning("PlayerMovementKeyBindingHintsPresenterScript: navigationHints array is null or empty!");
            navigationHints = System.Array.Empty<TextMeshPro>();
            directionToHint.Clear();
            return;
        }

        navigationHints = new TextMeshPro[navigationHintsObjects.Length];
        for (int i = 0; i < navigationHintsObjects.Length; i++)
        {
            if (navigationHintsObjects[i] != null)
                navigationHints[i] = navigationHintsObjects[i].GetComponent<TextMeshPro>();
        }

        directionToHint.Clear();

        if (isTurnOn)
        {
            foreach (var hint in navigationHints)
            {
                if (hint != null)
                    hint.gameObject.SetActive(true);
            }
        }
    }

    public void SetKeyBindings(MovementKeyBindingDataScript keyBindingDataNew)
    {
        keyBindingData = keyBindingDataNew;

        foreach (var pair in directionToHint)
        {
            if (pair.Value != null)
                pair.Value.text = GetKeyText(pair.Key);
        }
    }

    public void SetNavigationHint(Transform playerTransform, FaceScript faceScript)
    {
        if (!isTurnOn) return;

        if (navigationHints == null || navigationHints.Length == 0) 
        {
            Debug.Log("AAAAAAAAAAAAA");
            return;
        }

        FaceStateScript faceState = faceScript.FaceState;
        Transform faceTransform = faceScript.gameObject.transform;

        FaceProperty? direction = GetDirectionFromFaceState(faceState);
        if (!direction.HasValue) return;

        TextMeshPro textNavigationHint = GetOrAssignHint(direction.Value);
        if (textNavigationHint == null) return;

        textNavigationHint.text = GetKeyText(direction.Value);

        MeshRenderer renderer = textNavigationHint.GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.enabled = !faceState.GetFaceState(FaceProperty.IsBlocked);

        Transform navigationHintTransform = textNavigationHint.transform;
        navigationHintTransform.SetParent(faceTransform);
        navigationHintTransform.localPosition = new Vector3(0, extraHeight, 0);
        navigationHintTransform.localRotation = Quaternion.Euler(-90f, 90f, 0);

        TurnToThePlayer(playerTransform, navigationHintTransform);


    }

    private FaceProperty? GetDirectionFromFaceState(FaceStateScript faceState)
    {
        if (faceState.GetFaceState(FaceProperty.IsLeft)) return FaceProperty.IsLeft;
        if (faceState.GetFaceState(FaceProperty.IsRight)) return FaceProperty.IsRight;
        if (faceState.GetFaceState(FaceProperty.IsTop)) return FaceProperty.IsTop;
        return null;
    }

    private TextMeshPro GetOrAssignHint(FaceProperty direction)
    {
        if (directionToHint.TryGetValue(direction, out var hint) && hint != null)
            return hint;

        foreach (var availableHint in navigationHints)
        {
            if (availableHint == null) continue;

            bool alreadyAssigned = false;
            foreach (var assigned in directionToHint.Values)
            {
                if (assigned == availableHint)
                {
                    alreadyAssigned = true;
                    break;
                }
            }

            if (!alreadyAssigned)
            {
                directionToHint[direction] = availableHint;
                return availableHint;
            }
        }

        Debug.LogWarning($"PlayerMovementKeyBindingHintsPresenterScript: No available navigation hint to assign for direction {direction}!");
        return null;
    }

    private string GetKeyText(FaceProperty direction)
    {
        if (keyBindingData == null) return "?";

        return direction switch
        {
            FaceProperty.IsLeft => keyBindingData.keyLeft.ToString(),
            FaceProperty.IsRight => keyBindingData.keyRight.ToString(),
            FaceProperty.IsTop => keyBindingData.keyTop.ToString(),
            _ => "?"
        };
    }

    private void TurnToThePlayer(Transform playerTransform, Transform obj)
    {
        Vector3 look = obj.InverseTransformPoint(playerTransform.position);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg + 90f;
        obj.Rotate(0, 0, angle);
    }

    public void TurnOn()
    {
        isTurnOn = true;
        if (navigationHints != null)
        {
            foreach (var hint in navigationHints)
            {
                if (hint != null)
                    hint.gameObject.SetActive(true);
            }
        }
    }

    public void TurnOff()
    {
        isTurnOn = false;
        if (navigationHints != null)
        {
            foreach (var hint in navigationHints)
            {
                if (hint != null)
                    hint.gameObject.SetActive(false);
            }
        }
    }
}
