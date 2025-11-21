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

    protected void BuildIcosphere(float sideLen)
    {
        radiusIco = sideLen * 0.250000f * (Mathf.Sqrt(2.00000f * (5.0f + Mathf.Sqrt(5.00000f))));
        float radiusPenta = sideLen * (Mathf.Sqrt(10.00000f) * Mathf.Sqrt(5.0f + Mathf.Sqrt(5.00000f))) / 10.00000f;

        Vector3[] verticesIcosahedron = GetIcosahedronVertices(radiusIco, radiusPenta);

        //GetEdgeMidpoints
        //if (isTest) 
        GenerateInitialVerticies(verticesIcosahedron);

        GenerateFaces(verticesIcosahedron, sideLength);
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
