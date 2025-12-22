using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaramelDansenFloorScript : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject spritePrefab;
    public float brightnessStep = 0.2f; // Ўаг изменени€ €ркости
    public int brightnessLevels = 6; // от 0 до 5
    public Vector3 origin = Vector3.zero; // Ќачальна€ точка генерации

    private GameObject[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = origin + new Vector3(x, 0, z);
                GameObject obj = Instantiate(spritePrefab, position, Quaternion.Euler(90f, 0f, 0f), transform);
                grid[x, z] = obj;

                int level = Random.Range(0, brightnessLevels); // случайный уровень €ркости
                SetBrightness(obj, level);
            }
        }

    }

    void SetBrightness(GameObject obj, int level)
    {
        float brightness = Mathf.Clamp01(level * brightnessStep);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.color = new Color(brightness, brightness, brightness, 1f);
        obj.name = $"Sprite_{level}";
        obj.GetComponent<FloorSpriteData>().brightnessLevel = level;
    }

    public void RandomlyChangeBrightness()
    {
        foreach (GameObject obj in grid)
        {
            FloorSpriteData data = obj.GetComponent<FloorSpriteData>();
            int current = data.brightnessLevel;

            List<int> possibleLevels = new List<int>();
            if (current > 0) possibleLevels.Add(current - 1);
            possibleLevels.Add(current);
            if (current < brightnessLevels - 1) possibleLevels.Add(current + 1);

            int newLevel = possibleLevels[Random.Range(0, possibleLevels.Count)];
            SetBrightness(obj, newLevel);
        }
    }

    public IEnumerator HighlightBrightObjects()
    {
        foreach (GameObject obj in grid)
        {
            FloorSpriteData data = obj.GetComponent<FloorSpriteData>();
            if (data.brightnessLevel >= brightnessLevels - 2)
            {
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                sr.color = Color.Lerp(sr.color, Color.white, 0.5f);
            }
        }
        yield return null;
    }
}
