using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleRecursionBackgroundScript : MonoBehaviour
{
    public GameObject triangleSprite; // ”кажите спрайт треугольника в инспекторе
    public Transform laserShowHolder;
    private Color[] colors = { Color.white, Color.black };
    private int colorIndex = 0;
    private int count = 0;

    private void Start()
    {
        SpawnTriangle(colors[colorIndex]);
    }

    private void SpawnTriangle(Color color)
    {
        count++;
        GameObject triangle = Instantiate(triangleSprite, laserShowHolder);
        SpriteRenderer sr = triangle.GetComponent<SpriteRenderer>();
        sr.color = color;

        triangle.transform.localPosition = new Vector3(0, 0, -count/100000000000);
        triangle.transform.localRotation = Quaternion.identity;
        triangle.transform.localScale = Vector3.zero;

        TriangleGrower grower = triangle.AddComponent<TriangleGrower>();
        grower.spawner = this;
        grower.colorIndex = colorIndex;
    }

    public void OnTriangleReachesSize(int previousColorIndex)
    {
        colorIndex = (previousColorIndex + 1) % colors.Length;
        SpawnTriangle(colors[colorIndex]);
    }
}

public class TriangleGrower : MonoBehaviour
{
    public TriangleRecursionBackgroundScript spawner;
    public int colorIndex;
    private Vector3 maxSize = new Vector3(50,50, 0);
    private Vector3 spawnSize = new Vector3(10, 10, 0);
    private float growSpeed = 50f;

    private void Update()
    {
        transform.localScale += Vector3.one * growSpeed * Time.deltaTime;

        if (transform.localScale.x >= maxSize.x)
        {
            Destroy(gameObject);
        }
        else if (transform.localScale.x >= spawnSize.x && transform.localScale.x - growSpeed * Time.deltaTime < spawnSize.x)
        {
            spawner.OnTriangleReachesSize(colorIndex);
        }
    }
}
