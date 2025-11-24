using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FaceIcosphereBuilderScript : FaceIcosahedronBuilderScript
{
    [SerializeField] private int interation;
    private void Start()
    {
        StartCoroutine(BuildIcosphere(sideLength, interation));
    }

    protected IEnumerator BuildIcosphere(float sideLen, int iteration)
    {

        radiusIco = iteration * sideLen * 0.250000f * (Mathf.Sqrt(2.00000f * (5.0f + Mathf.Sqrt(5.00000f))));
        float radiusPenta = iteration * sideLen * (Mathf.Sqrt(10.00000f) * Mathf.Sqrt(5.0f + Mathf.Sqrt(5.00000f))) / 10.00000f;

        Vector3[] verticesIcosahedron = GetIcosahedronVertices(radiusIco, radiusPenta);

        Vector3[] combined = GetEdgeMidpoints(verticesIcosahedron, sideLen * iteration, iteration, radiusIco);

        combined = combined.Concat(verticesIcosahedron).ToArray();
        //if (isTest) 
        GenerateInitialVertices(AdjustVerticesToRadius(combined,radiusIco));
        Debug.Log("You have 10f");
        yield return new WaitForSeconds(10f);
        GenerateFaces(combined, 1.29375f, radiusIco);

    }

    public static Vector3[] GetEdgeMidpoints(Vector3[] vertices, float maxDistance, int iteration, float radius)
    {
        List<Vector3> midpoints = new();
        int x = 0;
        for (int abc = 0; abc < iteration-1; abc++)
        {
            x++;
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = i + 1; j < vertices.Length; j++)
                {
                    float distance = Vector3.Distance(vertices[i], vertices[j]);
                    if (Mathf.Abs(maxDistance - distance) <= 0.01f)
                    {
                        midpoints.Add((vertices[i] + vertices[j]) / 2);
                    }
                }
            }
            vertices = vertices.Concat(midpoints).ToArray();
            maxDistance /= 2f;
            
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
