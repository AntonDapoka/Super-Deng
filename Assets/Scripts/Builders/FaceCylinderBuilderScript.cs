using UnityEngine;

public class FaceCylinderBuilderScript : MonoBehaviour
{
    [SerializeField] private GameObject prefabFace;

    [Header("Grid Settings")]
    [SerializeField] private float radius = 5f;
    [SerializeField] private int gridWidth = 36; // Number of vertical strips (around)
    [SerializeField] private int gridHeight = 10; // Number of rows (height)
    [SerializeField] private float verticalSpacing = 1f;
    [SerializeField] private float alternateHeightOffset = 0.5f;

    private GameObject[,] faceGrid;

    public void GenerateCylindricalGrid()
    {
        if (prefabFace == null)
        {
            Debug.LogError("Triangle prefab is not assigned!");
            return;
        }

        GameObject cylinderGrid = new GameObject("CylindricalGrid");
        faceGrid = new GameObject[gridHeight, gridWidth];

        float angleStep = 360f / gridWidth;

        for (int y = 0; y < gridHeight; y++)
        {
            bool startWithFlipped = y % 2 == 1;

            for (int x = 0; x < gridWidth; x++)
            {
                bool isFlipped = (x % 2 == 1) ^ startWithFlipped;

                float angleDeg = x * angleStep;
                float angleRad = Mathf.Deg2Rad * angleDeg;

                float yPos = y * verticalSpacing + (isFlipped ? alternateHeightOffset : 0f);
                float xPos = Mathf.Cos(angleRad) * radius;
                float zPos = Mathf.Sin(angleRad) * radius;

                Vector3 position = new Vector3(xPos, yPos, zPos);

                GameObject triangle = Instantiate(prefabFace, position, Quaternion.identity, cylinderGrid.transform);

                // Повернуть треугольник так, чтобы он смотрел наружу от центра трубы
                Vector3 outward = new Vector3(xPos, 0f, zPos).normalized;
                triangle.transform.rotation = Quaternion.LookRotation(outward, Vector3.up);

                if (isFlipped)
                {
                    triangle.transform.Rotate(0f, 180f, 0f);
                }

                faceGrid[y, x] = triangle;
            }
        }
    }
}