using System.Collections.Generic;
using UnityEngine;

public class FaceIcosphereBuilderScript : FaceIcosahedronBuilderScript
{
    [SerializeField, Range(0, 2)]
    [Tooltip("0 = Icosahedron (20 faces), 1 = Icosphere (80 faces), 2 = Icosphere (320 faces)")]
    private int iteration = 1;

    private static Vector3[] baseTriangleVertices;
    private static bool baseTriangleCached;

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

            6, 3, 10,
            7, 4, 6,
            8, 5, 7,
            9, 1, 8,
            10, 2, 9,

            11, 7, 6,
            11, 8, 7,
            11, 9, 8,
            11, 10, 9,
            11, 6, 10
        };
    }

    protected override GameObject SetFace(Vector3[] verticesABC, Transform parent, int id)
    {
        if (verticesABC.Length != 3)
        {
            Debug.LogError("Are you eblan??? There is something wrong with vertices");
        }

        Vector3 center = (verticesABC[0] + verticesABC[1] + verticesABC[2]) / 3f;
        Vector3 vertexOnXAxis = verticesABC[0];
        Quaternion rotation = SetFaceRightRotation(vertexOnXAxis, verticesABC, center, Vector3.zero);

        GameObject face = Instantiate(facePrefab, center, rotation, parent);
        face.transform.localScale = new Vector3(faceScale, faceScale, faceScale);
        FaceScript faceScript = face.GetComponent<FaceScript>();
        faceScript.SetFaceID(id);
        GameObject shadow = faceScript.shadow;

        Vector3 A = verticesABC[0];
        Vector3 B = verticesABC[1];
        Vector3 C = verticesABC[2];
        Vector3 normal = Vector3.Cross(B - A, C - A).normalized;

        Vector3 shadowOffset = shadow.transform.position - face.transform.position;
        if (Vector3.Dot(normal, shadowOffset) > 0f) // shadow is on the outward side of the plane
        {
            face.transform.Rotate(0, 0, 180, Space.Self);
        }

        DeformFaceMeshes(face, verticesABC);

        return face;
    }

    private void DeformFaceMeshes(GameObject face, Vector3[] targetTriangle)
    {
        if (!baseTriangleCached)
        {
            CacheBaseTriangle();
        }

        if (baseTriangleVertices == null || baseTriangleVertices.Length != 3)
        {
            Debug.LogWarning("Base triangle not available. Skipping mesh deformation.");
            return;
        }

        Vector3 baseV0 = baseTriangleVertices[0];
        Vector3 baseV1 = baseTriangleVertices[1];
        Vector3 baseV2 = baseTriangleVertices[2];

        Vector3 localA = face.transform.InverseTransformPoint(targetTriangle[0]);
        Vector3 localB = face.transform.InverseTransformPoint(targetTriangle[1]);
        Vector3 localC = face.transform.InverseTransformPoint(targetTriangle[2]);

        Matrix4x4 deformMatrix = ComputeAffineTransform(baseV0, baseV1, baseV2, localA, localB, localC);

        foreach (var meshFilter in face.GetComponentsInChildren<MeshFilter>())
        {
            if (meshFilter.sharedMesh == null)
                continue;

            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            Matrix4x4 meshToFace = face.transform.worldToLocalMatrix * meshFilter.transform.localToWorldMatrix;
            Matrix4x4 faceToMesh = meshFilter.transform.worldToLocalMatrix * face.transform.localToWorldMatrix;
            Matrix4x4 combined = faceToMesh * deformMatrix * meshToFace;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = combined.MultiplyPoint3x4(vertices[i]);
            }

            Matrix4x4 normalMatrix = combined.inverse.transpose;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normalMatrix.MultiplyVector(normals[i]).normalized;
            }

            // Flip winding order to turn every face inside-out
            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
        }
    }

    private Matrix4x4 ComputeAffineTransform(Vector3 baseV0, Vector3 baseV1, Vector3 baseV2, Vector3 targetV0, Vector3 targetV1, Vector3 targetV2)
    {
        Vector3 baseE1 = baseV1 - baseV0;
        Vector3 baseE2 = baseV2 - baseV0;
        Vector3 baseN = Vector3.Cross(baseE1, baseE2).normalized;

        Vector3 targetE1 = targetV1 - targetV0;
        Vector3 targetE2 = targetV2 - targetV0;
        Vector3 targetN = Vector3.Cross(targetE1, targetE2).normalized;

        Matrix4x4 baseMatrix = Matrix4x4.identity;
        baseMatrix.SetColumn(0, new Vector4(baseE1.x, baseE1.y, baseE1.z, 0));
        baseMatrix.SetColumn(1, new Vector4(baseE2.x, baseE2.y, baseE2.z, 0));
        baseMatrix.SetColumn(2, new Vector4(baseN.x, baseN.y, baseN.z, 0));
        baseMatrix.SetColumn(3, new Vector4(baseV0.x, baseV0.y, baseV0.z, 1));

        Matrix4x4 targetMatrix = Matrix4x4.identity;
        targetMatrix.SetColumn(0, new Vector4(targetE1.x, targetE1.y, targetE1.z, 0));
        targetMatrix.SetColumn(1, new Vector4(targetE2.x, targetE2.y, targetE2.z, 0));
        targetMatrix.SetColumn(2, new Vector4(targetN.x, targetN.y, targetN.z, 0));
        targetMatrix.SetColumn(3, new Vector4(targetV0.x, targetV0.y, targetV0.z, 1));

        return targetMatrix * baseMatrix.inverse;
    }

    private void CacheBaseTriangle()
    {
        if (baseTriangleCached || facePrefab == null)
            return;

        GameObject temp = Instantiate(facePrefab);
        temp.transform.position = Vector3.zero;
        temp.transform.rotation = Quaternion.identity;
        temp.transform.localScale = Vector3.one;

        MeshFilter[] meshFilters = temp.GetComponentsInChildren<MeshFilter>();
        MeshFilter targetFilter = null;

        foreach (var mf in meshFilters)
        {
            if (mf.sharedMesh != null && mf.sharedMesh.vertexCount == 3)
            {
                targetFilter = mf;
                break;
            }
        }

        if (targetFilter == null)
        {
            Debug.LogError("Could not find a flat 3-vertex mesh in facePrefab. Mesh deformation will not work.");
            Destroy(temp);
            baseTriangleCached = true;
            baseTriangleVertices = null;
            return;
        }

        Vector3[] verts = targetFilter.sharedMesh.vertices;
        baseTriangleVertices = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            baseTriangleVertices[i] = targetFilter.transform.TransformPoint(verts[i]);
        }

        Destroy(temp);
        baseTriangleCached = true;
    }
}
