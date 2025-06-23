using Unity.VisualScripting;
using UnityEngine;

public class FaceGridGeneratorScript : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject trianglePrefab;
    private GameObject[,] faceGrid;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private BeatController BC;
    [SerializeField] private NavigationHintScript NHS;
    [SerializeField] private SoundScript SS;

    [Header("Grid Settings")]
    [SerializeField] private float horizontalSpacing = 0.9f;
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;

    [Header("Height Offsets")]
    [SerializeField] private float alternateHeightOffset = 0.5f;
    [SerializeField] private float rowHeightOffset = 2f;

    [Header("Questions")]
    [SerializeField] private bool havePlayer = false;
    [SerializeField] private bool isTutorial = false;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (trianglePrefab == null)
        {
            Debug.LogError("Triangle prefab is not assigned!");
            return;
        }
        faceGrid = new GameObject[gridHeight, gridWidth];

        for (int y = 0; y < gridHeight; y++)
        {
            bool startWithFlipped = y % 2 == 1;

            for (int x = 0; x < gridWidth; x++)
            {
                bool isFlipped = (x % 2 == 1) ^ startWithFlipped; // XOR
                float posX = x * horizontalSpacing;
                //float posY = isFlipped ? alternateHeightOffset : 0f;
                float posZ = y * rowHeightOffset + (isFlipped ? alternateHeightOffset : 0f);

                Vector3 position = new Vector3(posX, 0f, -posZ);
                GameObject triangle = Instantiate(trianglePrefab, position, Quaternion.identity, transform);

                if (isFlipped)
                {
                    triangle.transform.Rotate(0f, 180f, 0f);
                }

                faceGrid[y, x] = triangle;
            }
        }
        FAS.FindAllFaceScript();
        if (havePlayer) SetPlayer();
    }

    private void SetPlayer()
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                FaceScript FS = faceGrid[i, j].GetComponent<FaceScript>();
                FS.player = player;
                FS.SetFAS(FAS);
                FS.SetNHS(NHS);
                FS.SetBC(BC);
                FS.SetSS(SS);
                FS.isTutorial = isTutorial;
                FS.isTurnOn = true;
                FS.enabled = true;
                FS.enabled = true;
            }
        }

        int x = Random.Range((int)Mathf.Round(gridWidth * 0.4f), (int)Mathf.Round(gridWidth * 0.6f));
        int y = Random.Range((int)Mathf.Round(gridHeight * 0.4f), (int)Mathf.Round(gridHeight * 0.6f));
        
        player.transform.SetParent(faceGrid[x, y].transform);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.Euler(0, 180, 180);
        faceGrid[x, y].GetComponent<FaceScript>().havePlayer = true;
    }

    public GameObject[,] GetFaceGrid()
    {
        return faceGrid;
    }

    public FaceScript[,] GetFaceScriptGrid()
    {
        FaceScript[,] faceScripts = new FaceScript[gridHeight, gridWidth];

        for (int y = 0; y < gridHeight; y++)
        {

            for (int x = 0; x < gridWidth; x++)
            {
                faceScripts[y, x] = faceGrid[y, x].GetComponent<FaceScript>();
            }
        }
        return faceScripts;
    }

    public GameObject[,] GetFaceGlowingPartGrid()
    {
        GameObject[,] glowingPartGrid = new GameObject[gridHeight, gridWidth];

        for (int y = 0; y < gridHeight; y++)
        {

            for (int x = 0; x < gridWidth; x++)
            {
                glowingPartGrid[y, x] = faceGrid[y, x].GetComponentInChildren<GlowingPart>().gameObject;
            }
        }
        return glowingPartGrid;
    }
}
