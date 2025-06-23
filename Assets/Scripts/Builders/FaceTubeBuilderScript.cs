using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;


public class FaceTubeBuilderScript : MonoBehaviour
{
    [SerializeField] private GameObject prefabFace;

    private GameObject[] lineArray;

    [Header("Grid Settings")]
    [SerializeField] private float horizontalSpacing = 0.9f;
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;

    [Header("Height Offsets")]
    [SerializeField] private float alternateHeightOffset = 0.5f;
    [SerializeField] private float rowHeightOffset = 2f;
    [SerializeField] private float radius = 2f;

    public void GenerateGrid()
    {
        if (prefabFace == null)
        {
            Debug.LogError("Triangle prefab is not assigned!");
            return;
        }
        lineArray = new GameObject[gridHeight];

        GameObject grid = new GameObject("Grid");

        for (int y = 0; y < gridHeight; y++)
        {
            GameObject line = new GameObject("Line" + y);

            // Вычисляем угол
            float angle = y * (2f * Mathf.PI / gridHeight);
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);

            line.transform.position = position;
            line.transform.parent = grid.transform;

            lineArray[y] = line;
            
            bool startWithFlipped = y % 2 == 1;

            for (int x = 0; x < gridWidth; x++)
            {
                bool isFlipped = (x % 2 == 1) ^ startWithFlipped; // XOR
                float posZ = x * horizontalSpacing;
                float posX = (isFlipped ? -alternateHeightOffset : 0f); //y * rowHeightOffset + (isFlipped ? alternateHeightOffset : 0f);

                Vector3 worldPosition = new Vector3(posX, 0f, -posZ);
                GameObject triangle = Instantiate(prefabFace,  line.transform);
                triangle.transform.localPosition = worldPosition;
                triangle.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

                if (isFlipped)
                {
                    triangle.transform.Rotate(0f, 180f, 0f);
                }
            }
            /*
            Vector3 outward = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f).normalized;
            Vector3 tangent = Vector3.Cross(Vector3.forward, outward); // направление вдоль окружности

            lineArray[y].transform.rotation = Quaternion.LookRotation(-tangent, -outward);*/

            Vector3 outward = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f).normalized;
            line.transform.rotation = Quaternion.LookRotation(Vector3.forward, outward);
            line.transform.Rotate(0f, 0f, 180f); // если оси в префабе отличаются
        }


        /*
        for (int i = 0; i < gridHeight; i++)
        {
            // Вычисляем угол в радианах (равномерное распределение)
            float angle = i * (2f * Mathf.PI / gridHeight);

            // Позиция объекта на окружности (XZ-плоскость, Y=0)
            Vector3 spawnPosition = new Vector3(
                0f,
                Mathf.Cos(angle) * radius,
                
                Mathf.Sin(angle) * radius
            );

            // Создаем объект
            GameObject obj = Instantiate(prefabFace, spawnPosition, Quaternion.identity, transform);

            lineArray[i].transform.position = spawnPosition;    
        }*/
    }
}