using System.Collections.Generic;
using UnityEngine;

public class FaceIcosphereBuilderScript : FaceIcosahedronBuilderScript
{
    [SerializeField, Range(0, 2)]
    [Tooltip("0 = Icosahedron (20 faces), 1 = Icosphere (80 faces), 2 = Icosphere (320 faces)")]
    private int iteration = 1;

    public override void BuildField(GameObject newFacePrefab, float newSideLength, float newFaceScale)
    {
        facePrefab = newFacePrefab;
        sideLength = newSideLength;
        faceScale = newFaceScale;

        if (faces == null)
            faces = new List<GameObject>();
        else
            faces.Clear();

        BuildIcosphere(iteration);
    }

    private void BuildIcosphere(int subdivisions)
    {
        radiusIco = sideLength * 0.25f * Mathf.Sqrt(2f * (5f + Mathf.Sqrt(5f)));
        float radiusPenta = sideLength * (Mathf.Sqrt(10f) * Mathf.Sqrt(5f + Mathf.Sqrt(5f))) / 10f;

        Vector3[] baseVertices = GetIcosahedronVertices(radiusIco, radiusPenta);
        List<Vector3> vertices = new List<Vector3>(baseVertices);
        List<int> triangles = new List<int>(GetInitialTriangles());

        for (int s = 0; s < subdivisions; s++)
        {
            Subdivide(ref vertices, ref triangles, radiusIco);
        }

        if (isTest)
        {
            GenerateInitialVertices(vertices.ToArray());
        }

        fieldHolder = new GameObject("FieldHolder");
        fieldHolder.transform.position = Vector3.zero;

        int id = 0;
        for (int i = 0; i < triangles.Count; i += 3)
        {
            Vector3 a = vertices[triangles[i]];
            Vector3 b = vertices[triangles[i + 1]];
            Vector3 c = vertices[triangles[i + 2]];

            Vector3[] verticesABC = new Vector3[] { a, b, c };
            GameObject face = SetFace(verticesABC, fieldHolder.transform, id);
            faces.Add(face);
            id++;
        }
    }

    private void Subdivide(ref List<Vector3> vertices, ref List<int> triangles, float radius)
    {
        Dictionary<long, int> midpointCache = new Dictionary<long, int>();
        List<int> newTriangles = new List<int>(triangles.Count * 4);

        for (int i = 0; i < triangles.Count; i += 3)
        {
            int v0 = triangles[i];
            int v1 = triangles[i + 1];
            int v2 = triangles[i + 2];

            int a = GetMidpointIndex(v0, v1, vertices, radius, midpointCache);
            int b = GetMidpointIndex(v1, v2, vertices, radius, midpointCache);
            int c = GetMidpointIndex(v2, v0, vertices, radius, midpointCache);

            newTriangles.Add(v0); newTriangles.Add(a); newTriangles.Add(c);
            newTriangles.Add(v1); newTriangles.Add(b); newTriangles.Add(a);
            newTriangles.Add(v2); newTriangles.Add(c); newTriangles.Add(b);
            newTriangles.Add(a); newTriangles.Add(b); newTriangles.Add(c);
        }

        triangles = newTriangles;
    }

    private int GetMidpointIndex(int v1, int v2, List<Vector3> vertices, float radius, Dictionary<long, int> cache)
    {
        long key = ((long)Mathf.Min(v1, v2) << 32) | (uint)Mathf.Max(v1, v2);
        if (cache.TryGetValue(key, out int idx))
            return idx;

        Vector3 mid = (vertices[v1] + vertices[v2]) * 0.5f;
        mid = mid.normalized * radius;
        vertices.Add(mid);
        idx = vertices.Count - 1;
        cache[key] = idx;
        return idx;
    }

    private int[] GetInitialTriangles()
    {
        return new int[]
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5,
            0, 5, 1,

            1, 9, 2,
            2, 10, 3,
            3, 6, 4,
            4, 7, 5,
            5, 8, 1,

            6, 10, 3,
            7, 6, 4,
            8, 7, 5,
            9, 8, 1,
            10, 9, 2,

            11, 7, 6,
            11, 8, 7,
            11, 9, 8,
            11, 10, 9,
            11, 6, 10
        };
    }
}
