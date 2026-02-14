using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SpawnerActionScript : ActionScript, IPlayerInteractiveActionScript, IFieldInteractiveActionScript
{
    [SerializeField] private ActionType type;
    [SerializeField] protected GameObject[] faces;
    [SerializeField] protected bool isTurnOn = false;
    [SerializeField] protected bool isRandomSpawn = false;
    protected bool isCertainSpawn = false;
    protected bool isBasicSettingsChange = false;
    protected bool isStableQuantity;
    protected int quantityExact;
    protected int quantityMin;
    protected int quantityMax;

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
    public ActionType Type => type;


    public override void Initialize()
    {
        faces = FaceArray.GetAllFaces();
    }

    public override void Execute() 
    {

        List<int> availableFaces = GetAvailableFaces();
        if (isRandomSpawn)
        {
            int quantity = isStableQuantity ? quantityExact : Random.Range(quantityMin, quantityMax);

            for (int i = 0; i < quantity; i++)
            {
                if (availableFaces.Count == 0) return;

                int randomIndex = Random.Range(0, availableFaces.Count);
                int selectedFaceIndex = availableFaces[randomIndex];
                availableFaces.RemoveAt(randomIndex);

                SetActionFace(faces[selectedFaceIndex]); //Launch random ones from the available ones

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

            var intersectedIndices = source
            .Distinct()
            .Intersect(availableFaces)
            .ToList();

            foreach (int index in intersectedIndices)
            {
                availableFaces.RemoveAt(index);

                SetActionFace(faces[index]); //Launch the specified ones from the available ones
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
                availableFaces.Add(i);
            }
        }
        return availableFaces;
    }

    protected virtual bool CheckIsSuitableFace(FaceScript FS, FaceStateScript FSS)
    {
        bool res = //!FSS.Get(FaceProperty.HavePlayer) &&
                !FSS.Get(FaceProperty.IsBlinking) &&
                !FSS.Get(FaceProperty.IsKilling) &&
                !FSS.Get(FaceProperty.IsBlocked) &&
                !FSS.Get(FaceProperty.IsColored) &&
                !FSS.Get(FaceProperty.IsPortal) &&
                !FSS.Get(FaceProperty.IsBonus) &&
                //(isProximityLimit && FS.GetPathObjectCount() >= proximityLimit) &&
                //(isDistanceLimit && FS.GetPathObjectCount() <= distanceLimit) &&
                IsSuitableSpecialRequirements();
        return res;
    }

    public abstract bool IsSuitableSpecialRequirements();

    public abstract void SetBasicSettings(ActionBasicSettingsScript actionBasicSettings);

    public abstract void SetActionFace(GameObject gameObject);

    public override void Cancel() 
    { 
    
    }

    public override void TurnOn()
    {
        isTurnOn = true;
    }

    public override void TurnOff()
    {
        isTurnOn = false;
    }


}
