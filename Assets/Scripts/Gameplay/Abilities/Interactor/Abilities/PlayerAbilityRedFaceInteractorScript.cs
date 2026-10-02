using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAbilityRedFaceInteractorScript : PlayerAbilityScript, IHoldableAbilityScript
{
    [Header("RedFace Dependencies")]
    [SerializeField] private RedFaceBasicSettings redFaceBasicSettings;
    [SerializeField] private RedFaceSpawnerPresenterScript redFacePresenter;

    [Header("Input")]
    [SerializeField] private MovementKeyBindingDataScript movementKeyBindings;

    [Header("Events")]
    public UnityEvent OnBlockPlayerMovementOneBeat;

    private bool isAwaitingDirection;
    private GameObject currentFace;
    private readonly List<RedFaceScript> activeRedFaces = new();
    private readonly HashSet<GameObject> usedFaces = new();

    public override void Activate(GameObject face)
    {
        if (isAwaitingDirection) return;

        if (movementKeyBindings == null)
        {
            Debug.LogError("PlayerAbilityRedFaceInteractorScript MovementKeyBindings is not assigned!", this);
            return;
        }

        isAwaitingDirection = true;
        currentFace = face;
        usedFaces.Clear();
    }

    public void Release()
    {
        isAwaitingDirection = false;
        currentFace = null;
        usedFaces.Clear();
    }

    private void Update()
    {
        for (int i = activeRedFaces.Count - 1; i >= 0; i--)
        {
            activeRedFaces[i].Update();
            if (activeRedFaces[i].IsFinished)
                activeRedFaces.RemoveAt(i);
        }

        if (!isAwaitingDirection || currentFace == null) return;

        string direction = null;

        if (Input.GetKeyDown(movementKeyBindings.keyLeft))
            direction = "Left";
        else if (Input.GetKeyDown(movementKeyBindings.keyRight))
            direction = "Right";
        else if (Input.GetKeyDown(movementKeyBindings.keyTop))
            direction = "Top";

        if (direction != null)
        {
            SpawnRedFace(direction);
        }
    }

    private void SpawnRedFace(string direction)
    {
        if (redFaceBasicSettings == null || redFacePresenter == null)
        {
            Debug.LogError("PlayerAbilityRedFaceInteractorScript Missing RedFace dependencies!", this);
            return;
        }

        FaceScript faceScript = currentFace.GetComponent<FaceScript>();
        GameObject targetFace = GetNeighborInDirection(faceScript, direction);

        if (targetFace == null)
        {
            Debug.LogWarning($"PlayerAbilityRedFaceInteractorScript No neighbor face found in direction: {direction}", this);
            return;
        }

        if (usedFaces.Contains(targetFace))
        {
            Debug.LogWarning("PlayerAbilityRedFaceInteractorScript Target face was already used in this hold.", this);
            return;
        }

        if (!IsFaceSuitable(targetFace.GetComponent<FaceStateScript>()))
        {
            Debug.LogWarning("PlayerAbilityRedFaceInteractorScript Target face is not suitable for RedFace.", this);
            return;
        }

        RedFaceScript redFace = new(targetFace, null, redFaceBasicSettings, redFacePresenter);
        activeRedFaces.Add(redFace);
        usedFaces.Add(targetFace);
        OnBlockPlayerMovementOneBeat?.Invoke();
    }

    private GameObject GetNeighborInDirection(FaceScript face, string direction)
    {
        FaceProperty targetProp = direction switch
        {
            "Left" => FaceProperty.IsLeft,
            "Right" => FaceProperty.IsRight,
            "Top" => FaceProperty.IsTop,
            _ => FaceProperty.HavePlayer
        };

        GameObject[] sides = new[] { face.side1, face.side2, face.side3 };
        foreach (var side in sides)
        {
            if (side == null) continue;
            FaceStateScript state = side.GetComponent<FaceStateScript>();
            if (state.GetFaceState(targetProp))
                return side;
        }

        return null;
    }

    private bool IsFaceSuitable(FaceStateScript state)
    {
        return !state.GetFaceState(FaceProperty.IsBlinking) &&
               !state.GetFaceState(FaceProperty.IsKilling) &&
               !state.GetFaceState(FaceProperty.IsBlocked) &&
               !state.GetFaceState(FaceProperty.IsColored) &&
               !state.GetFaceState(FaceProperty.IsPortal) &&
               !state.GetFaceState(FaceProperty.IsBonus);
    }
}
