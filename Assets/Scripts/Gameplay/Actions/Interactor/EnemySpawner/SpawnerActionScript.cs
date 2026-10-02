using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SpawnerActionScript : ActionScript, IBeatUpdate, IPlayerInteractiveActionScript, IFieldInteractiveActionScript
{
    [SerializeField] protected SpawnerActionPresenterScript presenter;
    [SerializeField] protected GameObject[] faces;
    [SerializeField] protected bool isTurnOn = false;
    [SerializeField] protected bool isForcedBreak = false;
    [SerializeField] protected bool isRandomSpawn = false;
    protected bool isCertainSpawn = false;
    protected bool isBasicSettingsChange = false;
    protected bool isStableQuantity;
    protected float quantityExact;
    protected float quantityMin;
    protected float quantityMax;
    [SerializeField] protected float quantityAccumulator = 0f;

    protected bool isRelativeToPlayer = false;
    protected int[] arrayOfFacesRelativeToPlayer;
    protected bool isRelativeToFigure = false;
    protected int[] arrayOfFacesRelativeToFigure;

    protected bool isProximityLimit = false;
    protected int proximityLimit;
    protected bool isDistanceLimit = false;
    protected int distanceLimit;

    [SerializeField] private PlayerStateInteractorScript playerStateInteractor;
    [SerializeField] private FieldInteractorScript fieldInteractor;
    [SerializeField] private FaceArrayScript faceArray;

    public PlayerStateInteractorScript PlayerStatetInteractor => playerStateInteractor;
    public FieldInteractorScript FieldInteractor => fieldInteractor;
    public FaceArrayScript FaceArray => faceArray;

    public override void Initialize()
    {
        faces = FaceArray.GetAllFaces();
        Debug.Log(faces.Length.ToString() + "Initialized");
    }

    public void OnBeat()
    {
        Execute();
    }

    public override void Execute() 
    {
        if (!isTurnOn) return;

        List<int> availableFaces = GetAvailableFaces();
        if (isRandomSpawn)
        {
            float quantity = isStableQuantity ? quantityExact : Random.Range(quantityMin, quantityMax);
            quantityAccumulator += quantity;

            while (quantityAccumulator >= 1f)
            {
                if (availableFaces.Count == 0) 
                {
                    Debug.Log("No available faces!");
                    return;
                }
                int randomIndex = Random.Range(0, availableFaces.Count);
                int selectedFaceId = availableFaces[randomIndex];
                availableFaces.RemoveAt(randomIndex);

                GameObject targetFace = FaceArray.GetFaceByID(selectedFaceId);
                if (targetFace != null)
                    SetActionFace(targetFace); //Launch random ones from the available ones
                quantityAccumulator -= 1f;
            }
        }
        if (isCertainSpawn) 
        {
            if (!isRelativeToPlayer && !isRelativeToFigure)
                return;

            IEnumerable<int> source = Enumerable.Empty<int>();

            if (isRelativeToPlayer)
                source = source.Concat(arrayOfFacesRelativeToPlayer);

            if (isRelativeToFigure)
                source = source.Concat(arrayOfFacesRelativeToFigure);

            var intersectedFaceIds = source
            .Distinct()
            .Intersect(availableFaces)
            .ToList();

            foreach (int faceId in intersectedFaceIds)
            {
                availableFaces.Remove(faceId);

                GameObject targetFace = FaceArray.GetFaceByID(faceId);
                if (targetFace != null)
                    SetActionFace(targetFace); //Launch the specified ones from the available ones
            }
        }
    }

    public virtual List<int> GetAvailableFaces()
    {
        List<int> availableFaces = new(); 

        for (int i = 0; i < faces.Length; i++)
        {
            FaceScript FS = faces[i].GetComponent<FaceScript>();
            FaceStateScript FSS = faces[i].GetComponent<FaceStateScript>();
            if (CheckIsSuitableFace(FS, FSS))
            {
                availableFaces.Add(FS.GetFaceID());
            }
        }
        return availableFaces;
    }

    protected virtual bool CheckIsSuitableFace(FaceScript FS, FaceStateScript FSS)
    {
        bool res = //!FSS.GetFaceState(FaceProperty.HavePlayer) &&
                !FSS.GetFaceState(FaceProperty.IsBlinking) &&
                !FSS.GetFaceState(FaceProperty.IsKilling) &&
                !FSS.GetFaceState(FaceProperty.IsBlocked) &&
                !FSS.GetFaceState(FaceProperty.IsColored) &&
                !FSS.GetFaceState(FaceProperty.IsPortal) &&
                !FSS.GetFaceState(FaceProperty.IsBonus) &&
                (!isProximityLimit || FS.GetPathObjectCount() >= proximityLimit) &&
                (!isDistanceLimit || FS.GetPathObjectCount() <= distanceLimit) &&
                IsSuitableSpecialRequirements(FS, FSS);
        return res;
    }

    public abstract bool IsSuitableSpecialRequirements(FaceScript FS, FaceStateScript FSS);

    public abstract void SetActionFace(GameObject gameObject);

    public override void Cancel() {}

    public override void ForcedBreak() {}

    public override void TurnOn()
    {
        isTurnOn = true;
    }

    public override void TurnOff()
    {
        isTurnOn = false;
    }
}
