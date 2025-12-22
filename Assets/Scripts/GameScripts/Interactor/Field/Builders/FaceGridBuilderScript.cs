using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FaceGridBuilderScript : MonoBehaviour
{
    [SerializeField] private GameObject prefabFace;
    private GameObject[,] faceGrid;

    [Header("Grid Settings")]
    [SerializeField] private float horizontalSpacing = 0.9f;
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;

    [Header("Height Offsets")]
    [SerializeField] private float alternateHeightOffset = 0.5f;
    [SerializeField] private float rowHeightOffset = 2f;

    public void GenerateGrid()
    {
        if (prefabFace == null)
        {
            Debug.LogError("Triangle prefab is not assigned!");
            return;
        }

        faceGrid = new GameObject[gridHeight, gridWidth];
        GameObject grid = new GameObject("Grid");

        // Список всех позиций для вычисления центра
        List<Vector3> positions = new List<Vector3>();

        for (int y = 0; y < gridHeight; y++)
        {
            GameObject line = new GameObject("Line" + y);
            line.transform.parent = grid.transform;

            bool startWithFlipped = y % 2 == 1;

            for (int x = 0; x < gridWidth; x++)
            {
                bool isFlipped = (x % 2 == 1) ^ startWithFlipped; // XOR
                float posX = x * horizontalSpacing;
                float posZ = y * rowHeightOffset + (isFlipped ? alternateHeightOffset : 0f);

                Vector3 worldPosition = new Vector3(posX, 0f, -posZ);
                GameObject triangle = Instantiate(prefabFace, worldPosition, Quaternion.identity, line.transform);

                if (isFlipped)
                {
                    triangle.transform.Rotate(0f, 180f, 0f);
                }

                faceGrid[y, x] = triangle;
                positions.Add(worldPosition);
            }
        }

        // Вычисление центра
        Vector3 total = Vector3.zero;
        foreach (Vector3 pos in positions)
            total += pos;

        Vector3 center = total / positions.Count;

        foreach (Transform child in grid.transform)
            child.localPosition -= center;

        grid.transform.position = Vector3.zero;
        grid.transform.rotation = Quaternion.Euler(-30f, 0f, 0f);
    }
}
