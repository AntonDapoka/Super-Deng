using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SpawnerActionScript : MonoBehaviour, IPlayerInteractiveActionScript, IFieldInteractiveActionScript
{
    [SerializeField] private ActionType type;
    protected GameObject[] faces;
    protected List<int> faceIndices = new();
    protected float bpm;
    protected int quantity;
    protected bool isTurnOn = false;
    protected bool isRandomSpawn = false;

    [SerializeField] private PlayerStateInteractorScript playerStateInteractor;
    [SerializeField] private FieldInteractorScript fieldInteractor;
    [SerializeField] private FaceArrayScript faceArray;

    public PlayerStateInteractorScript PlayerStatetInteractor => playerStateInteractor;
    public FieldInteractorScript FieldInteractor => fieldInteractor;
    public FaceArrayScript FaceArray => faceArray;
    public ActionType Type => type;

    public void TurnOn()
    {
        isTurnOn = true;    
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }

    public virtual void Initialize()
    {
        faces = FaceArray.GetAllFaces();
    }

    public abstract void SetSettings<T>(T settings);

    public virtual void Execute() 
    {
        List<int> availableFaces = GetAvailableFaces();
        if (isRandomSpawn)
        {
            for (int i = 0; i < quantity; i++)
            {
                if (availableFaces.Count == 0) return;

                int randomIndex = Random.Range(0, availableFaces.Count);
                int selectedFaceIndex = availableFaces[randomIndex];
                availableFaces.RemoveAt(randomIndex);

                SetActionFace(faces[selectedFaceIndex]); //Launch random ones from the available ones
            }
        }
        else
        {
            var intersectedIndices = faceIndices.Intersect(availableFaces);
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
            FaceStateScript FSS = faces[i].GetComponent<FaceStateScript>();
            if (CheckIsSuitableFace(FSS))
            {
                availableFaces.Add(i);
            }
        }
        return availableFaces;
    }

    protected virtual bool CheckIsSuitableFace(FaceStateScript FSS)
    {
        bool res = //!FSS.Get(FaceProperty.HavePlayer) &&
                !FSS.Get(FaceProperty.IsBlinking) &&
                !FSS.Get(FaceProperty.IsKilling) &&
                !FSS.Get(FaceProperty.IsBlocked) &&
                !FSS.Get(FaceProperty.IsColored) &&
                !FSS.Get(FaceProperty.IsPortal) &&
                !FSS.Get(FaceProperty.IsBonus) &&
                IsSuitableSpecialRequirements();
        return res;
    }

    public abstract bool IsSuitableSpecialRequirements();

    public abstract void SetActionFace(GameObject gameObject);

    public virtual void Cancel() { }
}
