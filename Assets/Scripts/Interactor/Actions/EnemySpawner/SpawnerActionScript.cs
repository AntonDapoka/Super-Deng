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

    [SerializeField] private PlayerInteractorScript _playerInteractor;
    [SerializeField] private FieldInteractorScript _fieldInteractor;
    [SerializeField] private MonoBehaviour _rhythmable;

    public PlayerInteractorScript PlayerInteractor => _playerInteractor;
    public FieldInteractorScript FieldInteractor => _fieldInteractor;
    public IRhythmableScript Rhythmable => _rhythmable as IRhythmableScript;

    public virtual void Initialize()
    {
        faces = FieldInteractor.GetAllFaces();
    }
    

    public virtual void Execute() 
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
        bool res = !FSS.HavePlayer &&
                !FSS.IsBlinking &&
                !FSS.IsKilling &&
                !FSS.IsBlocked &&
                !FSS.IsColored &&
                !FSS.IsPortal &&
                !FSS.IsBonus;
        return res;
    }

    public virtual void Cancel() { }
}
