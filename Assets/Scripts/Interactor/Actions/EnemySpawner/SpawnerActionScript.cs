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
        faces = _fieldInteractor.GetAllFaces();
        List<int> availableFaces = new List<int>(); //Составляем массив из доступных граней

        for (int i = 0; i < faces.Length; i++)
        {
            FaceScript FS = faces[i].GetComponent<FaceScript>();
            if (!FS.havePlayer &&
                !FS.isBlinking &&
                !FS.isKilling &&
                !FS.isBlocked &&
                !FS.isColored &&
                !FS.isPortal &&
                !FS.isBonus)
            {
                availableFaces.Add(i);
            }
        }

        return availableFaces;
    }

    public void Cancel() { }
}
