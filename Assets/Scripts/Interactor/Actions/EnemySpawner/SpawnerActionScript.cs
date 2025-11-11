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

    public void Initialize()
    { }

    

    public void Execute() 
    {
        List<int> availableFaces = GetAvailableFaces();
        if (isRandomSpawnTime)
        {
            for (int i = 0; i < colvo; i++)
            {
                if (availableFaces.Count == 0) return;

                int randomIndex = Random.Range(0, availableFaces.Count);
                int selectedFaceIndex = availableFaces[randomIndex];
                //StartCoroutine(SetEnemy(faces[selectedFaceIndex])); //Запускаем рандомные из доступных
                availableFaces.RemoveAt(randomIndex);
            }
        }
        else
        {
            var intersectedIndices = faceIndices.Intersect(availableFaces);
            foreach (int index in intersectedIndices)
            {
                //StartCoroutine(SetEnemy(faces[index])); //Запускаем указанные из доступных
                availableFaces.RemoveAt(index);
            }
        }
    }

    private List<int> GetAvailableFaces()
    {
        faces = FieldInteractor.GetAllFaces();
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

    private bool CheckIsSuitableFace(FaceStateScript FSS)
    {
        bool res = !FSS.havePlayer &&
                !FSS.isBlinking &&
                !FSS.isKilling &&
                !FSS.isBlocked &&
                !FSS.isColored &&
                !FSS.isPortal &&
                !FSS.isBonus;
        return res;
    }

    public void Cancel() { }
}
