using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceIcosphereBuilderScript : FaceIcosahedronBuilderScript
{
    protected float epsilon = 0.001f;
    private void Start()
    {
        BuildIcosphere(sideLength, 2);
    }

    protected void BuildIcosphere(float sideLen, int iteration)
    {

        radiusIco = iteration * sideLen * 0.250000f * (Mathf.Sqrt(2.00000f * (5.0f + Mathf.Sqrt(5.00000f))));
        float radiusPenta = iteration * sideLen * (Mathf.Sqrt(10.00000f) * Mathf.Sqrt(5.0f + Mathf.Sqrt(5.00000f))) / 10.00000f;

        Vector3[] verticesIcosahedron = GetIcosahedronVertices(radiusIco, radiusPenta);
        Vector3[] combined = verticesIcosahedron;

        combined = combined.Concat(GetEdgeMidpoints(verticesIcosahedron, sideLen * iteration, radiusIco)).ToArray();
        //if (isTest) 
        GenerateInitialVerticies(combined);

        //GenerateFaces(verticesIcosahedron, sideLength);
        GenerateFaces(combined, sideLength, radiusIco);
    }

    public static Vector3[] GetEdgeMidpoints(Vector3[] vertices, float maxDistance, float radius)
    {
        HashSet<(int, int)> edges = new();
        List<Vector3> midpoints = new();

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                float distance = Vector3.Distance(vertices[i], vertices[j]);
                if (Mathf.Abs(maxDistance - distance) <= 0.01f)
                {
                    edges.Add((i, j));
                    midpoints.Add((vertices[i] + vertices[j]) / 2);
                }
            }
        }
        return midpoints.ToArray();
    }


    /*s


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
    }*/
}
