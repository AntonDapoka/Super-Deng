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


        //if (isTest) 
        GenerateInitialVertices(combined);
        Debug.Log("You have 1f");
        yield return new WaitForSeconds(1f);
        StartCoroutine(GeneratingFaces(combined, sideLen, radiusIco));

    }

    protected IEnumerator GeneratingFaces(Vector3[] vertices, float maxDistance, float radius)
    {
        int w = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                for (int k = j + 1; k < vertices.Length; k++)
                {
                    Vector3 a = vertices[i];
                    Vector3 b = vertices[j];
                    Vector3 c = vertices[k];
                    float abDistance = Vector3.Distance(a, b);
                    float bcDistance = Vector3.Distance(b, c);
                    float acDistance = Vector3.Distance(c, a);
                    Debug.Log(abDistance.ToString() + " " + maxDistance);
                    if (Mathf.Abs(maxDistance - abDistance) <= epsilon &&
                        Mathf.Abs(maxDistance - bcDistance) <= epsilon &&
                        Mathf.Abs(maxDistance - acDistance) <= epsilon)
                    {
                        w++;
                        Vector3[] verticesABC = AdjustVerticesToRadius(new Vector3[3] { a, b, c }, radius);
                        GameObject face = SetFace(verticesABC);
                        yield return new WaitForSeconds(0.5f);
                        //faces.Add(face);
                    }
                }
            }
        }
        Debug.Log(w);
        //GroupGameObjects(faces.ToArray());
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
            //vertices = vertices.Concat(midpoints).ToArray();
            maxDistance /= 2f;
            
        }

        Vector3[] combined = midpoints.Concat(vertices).ToArray();
        return combined;
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
