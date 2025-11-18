using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceIcosphereBuilderScript : FaceIcosahedronBuilderScript
{
    [SerializeField] private int iter;
    [SerializeField] private GameObject prismPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        radiusIco = sideLength * 0.250000f * (Mathf.Sqrt(2.00000f * (5.0f + Mathf.Sqrt(5.00000f))));
        float radiusPenta = sideLength * (Mathf.Sqrt(10.00000f) * Mathf.Sqrt(5.0f + Mathf.Sqrt(5.00000f))) / 10.00000f;

        Vector3[] verticesIcosahedron = GetIcosahedronVertices(radiusIco, radiusPenta);
    }

    public IEnumerator GenerateIcosphere(Vector3[] vertices, float radiusIco) //Change to void after completion
    {
        Vector3[] combined = vertices;

        // float[] distances = GenerateDistances(iterations + 1, sideLength);

        List<GameObject> gameObjects = new List<GameObject>();

        foreach (var vertice in vertices)
        {
            GameObject sphere = Instantiate(prismPrefab, vertice, Quaternion.identity);
            sphere.transform.SetParent(gameObject.transform);
            gameObjects.Add(sphere);
        }

        //GroupGameObjects(gameObjects.ToArray());
        /*
        Debug.Log($"Iteration {0}: {combined.Length} vertices");

        for (int i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(2f);

            Vector3[] midPoints = GetEdgeMidpoints(combined, distances[i]);
            //Vector3[] extraVertices = AdjustMidpointsToRadius(midPoints, radiusIco);

            foreach (var vertice in extraVertices)
            {
                Instantiate(prismPrefab, vertice, Quaternion.identity);
            }

            combined = combined.Concat(extraVertices).ToArray();
            Debug.Log($"Iteration {i + 1}: {combined.Length} vertices with distance {distances[i]}");

        }
        Debug.Log(iterations);        */
        //GenerateFaces(combined, distances[iterations], iterations);

        yield return new WaitForSeconds(2f);
    }

    public static Vector3[] GetEdgeMidpoints(Vector3[] vertices, float maxDistance)
    {
        HashSet<(int, int)> edges = new();
        List<Vector3> midpoints = new();

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



    public static float[] GenerateDistances(int iterations, float a)
    {
        float[] distances = new float[iterations];
        float currentDistance = a;

        for (int i = 0; i < iterations; i++)
        {
            distances[i] = currentDistance * 1.25f;
            currentDistance /= 2f;

        }

        return distances;
    }
}
