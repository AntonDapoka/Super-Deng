using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SpawnerActionScript : IPlayerInteractiveActionScript, IFieldInteractiveActionScript
{
    private GameObject[] faces;
    public List<int> faceIndices = new();
    private int colvo;
    private bool isRandomSpawnTime = false;

    [SerializeField] private PlayerStateInteractorScript _playerStateInteractor;
    [SerializeField] private FieldInteractorScript _fieldInteractor;
    [SerializeField] private MonoBehaviour _rhythmable;

    public PlayerStateInteractorScript PlayerStatetInteractor => _playerStateInteractor;
    public FieldInteractorScript FieldInteractor => _fieldInteractor;
    public IRhythmableScript Rhythmable => _rhythmable as IRhythmableScript;

    public virtual void Initialize()
    {
        //faces = FieldInteractor.GetAllFaces();
    }
    

    public virtual void Execute(object definition) 
    {
        List<int> availableFaces = GetAvailableFaces();
        if (isRandomSpawnTime)
        {
            for (int i = 0; i < colvo; i++)
            {
                if (availableFaces.Count == 0) return;

                int randomIndex = Random.Range(0, availableFaces.Count);
                int selectedFaceIndex = availableFaces[randomIndex];
                availableFaces.RemoveAt(randomIndex);

                SetEnemy(faces[selectedFaceIndex]); //Launch random ones from the available ones
            }
        }
        else
        {
            var intersectedIndices = faceIndices.Intersect(availableFaces);
            foreach (int index in intersectedIndices)
            {
                availableFaces.RemoveAt(index);

                SetEnemy(faces[index]); //Launch the specified ones from the available ones
            }
        }
    }

    public abstract void SetEnemy(GameObject gameObject);

    public virtual List<int> GetAvailableFaces()
    {
        
        List<int> availableFaces = new List<int>(); //Create an array of available faces

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

    public virtual bool CheckIsSuitableFace(FaceStateScript FSS)
    {
        bool res = !FSS.Get(FaceProperty.HavePlayer) &&
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

    public virtual void Cancel(object definition) { }
}
