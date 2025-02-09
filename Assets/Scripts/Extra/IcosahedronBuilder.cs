using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IcosahedronBuilder : MonoBehaviour
{
    [SerializeField] private GameObject prismPrefab; 
    [SerializeField] private GameObject facePrefab;
    [SerializeField] private int inter; 
    private float radiusIco; 
    //[SerializeField] private float prismScaleFactor = 0.9f;

    private void Start()
    {
        BuildIcosahedron();
    }

    private void BuildIcosahedron()
    {
        float a = 1.00000f;
        radiusIco = a * 0.250000f * (Mathf.Sqrt(2.00000f * (5.0f + Mathf.Sqrt(5.00000f))));
        float radiusPenta = a * (Mathf.Sqrt(10.00000f) * Mathf.Sqrt(5.0f + Mathf.Sqrt(5.00000f))) / 10.00000f;


        Vector3[] verticesIcosahedron = new Vector3[]
        {
            new(0, radiusIco, 0),
            new(-radiusPenta, radiusPenta / 2, 0),
            new(-Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta),
            new(Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta),
            new(Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, -Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta),
            new(-Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, -Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta),

            new(radiusPenta, -radiusPenta / 2, 0),
            new(Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, -Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta),
            new(-Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, -Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta),
            new(-Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta),
            new(Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta),
            new(0, -radiusIco, 0)
        };

        Debug.Log(verticesIcosahedron[0]);
        Debug.Log(verticesIcosahedron[1]);
        Debug.Log(verticesIcosahedron[2]);
        foreach (var vertice in verticesIcosahedron)
        {
            
            Instantiate(prismPrefab, vertice, Quaternion.identity);
        }

        //StartCoroutine(GenerateIcosphere(verticesIcosahedron, radiusIco, inter));

        Vector3[] centers = GenerateCenters(verticesIcosahedron, 1.1f);
        Quaternion[] quaternions = GenerateRotation(verticesIcosahedron, 1.1f);

        //for (int j = 0; j < centers.Length; j++)
        //{

        Debug.Log(centers[0]);
        Debug.Log(quaternions[0]);
        GameObject gameobject = Instantiate(facePrefab, centers[0], quaternions[0]);
            //gameobject.GetComponent<FaceScript>().enabled = false;
        //}
    }

    public IEnumerator GenerateIcosphere(Vector3[] vertices, float radiusIco, int iterations)
    {
        Vector3[] combined = vertices;

        float[] distances = GenerateDistances(iterations);

        for (int i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(2f);
            Vector3[] midPoints = GetEdgeMidpoints(combined, distances[i]);
            Vector3[] extraVertices = AdjustMidpointsToRadius(midPoints, radiusIco);

            foreach (var vertice in extraVertices)
            {
                Instantiate(prismPrefab, vertice, Quaternion.identity);
            }

            combined = combined.Concat(extraVertices).ToArray();
            Debug.Log($"Iteration {i + 1}: {combined.Length} vertices");
        }

        
    }

    public static Vector3[] GetEdgeMidpoints(Vector3[] vertices, float maxDistance)
    {
        HashSet<(int, int)> edges = new HashSet<(int, int)>();
        List<Vector3> midpoints = new List<Vector3>();

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                float distance = Vector3.Distance(vertices[i], vertices[j]);
                if (distance < maxDistance)
                {
                    edges.Add((i, j));
                    midpoints.Add((vertices[i] + vertices[j]) / 2);
                }
            }
        }
        return midpoints.ToArray();
    }

    public static Vector3[] AdjustMidpointsToRadius(Vector3[] midpoints, float radius)
    {
        for (int i = 0; i < midpoints.Length; i++)
        {
            midpoints[i] = midpoints[i].normalized * radius;
        }
        return midpoints;
    }
    public static float[] GenerateDistances(int iterations)
    {
        float[] distances = new float[iterations];
        float currentDistance = 1f;

        for (int i = 0; i < iterations; i++)
        {
            distances[i] = currentDistance;
            currentDistance *= 1.1f;  // Multiply by 1.1 for each iteration
            if (i % 2 == 1)  // Half the value every second iteration (as per the logic in your description)
            {
                currentDistance /= 2f;
            }
        }

        return distances;
    }

    public Vector3[] GenerateCenters(Vector3[] vertices, float maxDistance)
    {
        List<Vector3> centers = new();

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                for (int k = j + 1; k < vertices.Length; k++)
                {
                    if (Vector3.Distance(vertices[i], vertices[j]) <= maxDistance &&
                        Vector3.Distance(vertices[j], vertices[k]) <= maxDistance &&
                        Vector3.Distance(vertices[k], vertices[i]) <= maxDistance)
                    {
                        Vector3 center = (vertices[i] + vertices[j] + vertices[k]) / 3f;
                        centers.Add(center);
                    }
                }
            }
        }

        return centers.ToArray();
    }

    public Quaternion[] GenerateRotation(Vector3[] vertices, float maxDistance)
    {
        List<Quaternion> quaternions = new();

        // Проходим по всем вершинам
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                for (int k = j + 1; k < vertices.Length; k++)
                {
                    // Проверяем расстояния между вершинами
                    if (Vector3.Distance(vertices[i], vertices[j]) <= maxDistance &&
                        Vector3.Distance(vertices[j], vertices[k]) <= maxDistance &&
                        Vector3.Distance(vertices[k], vertices[i]) <= maxDistance)
                    {
                        // Если все расстояния меньше или равны maxDistance, добавляем треугольник
                        Vector3 v1 = vertices[j] - vertices[i];
                        Vector3 v2 = vertices[k] - vertices[i];
                        Vector3 normal = Vector3.Cross(v1, v2).normalized;

                        float angleX = Mathf.Acos(Vector3.Dot(normal, Vector3.right)) * Mathf.Rad2Deg;
                        float angleY = Mathf.Acos(Vector3.Dot(normal, Vector3.up)) * Mathf.Rad2Deg;
                        float angleZ = Mathf.Acos(Vector3.Dot(normal, Vector3.forward)) * Mathf.Rad2Deg;

                        Quaternion rotation = Quaternion.Euler(angleX, angleY, angleZ);

                        quaternions.Add(rotation);
                    }
                }
            }
        }

        return quaternions.ToArray();
    }
}
