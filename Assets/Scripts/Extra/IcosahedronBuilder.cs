using UnityEngine;

public class IcosahedronBuilder : MonoBehaviour
{
    [SerializeField] private GameObject prismPrefab; 
    [SerializeField] private float scale = 1.0f; 
    [SerializeField] private float prismScaleFactor = 0.9f;

    private void Start()
    {
        BuildIcosahedron();
    }

    private void BuildIcosahedron()
    {
        float phi = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1,  phi,  0),
            new Vector3( 1,  phi,  0),
            new Vector3(-1, -phi,  0),
            new Vector3( 1, -phi,  0),
            new Vector3( 0, -1,  phi),
            new Vector3( 0,  1,  phi),
            new Vector3( 0, -1, -phi),
            new Vector3( 0,  1, -phi),
            new Vector3( phi,  0, -1),
            new Vector3( phi,  0,  1),
            new Vector3(-phi,  0, -1),
            new Vector3(-phi,  0,  1)
        };
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].normalized * scale;
        }
        int[][] triangles = new int[][]
        {
            new int[] { 0, 11, 5 },
            new int[] { 0, 5, 1 },
            new int[] { 0, 1, 7 },
            new int[] { 0, 7, 10 },
            new int[] { 0, 10, 11 },
            new int[] { 1, 5, 9 },
            new int[] { 5, 11, 4 },
            new int[] { 11, 10, 2 },
            new int[] { 10, 7, 6 },
            new int[] { 7, 1, 8 },
            new int[] { 3, 9, 4 },
            new int[] { 3, 4, 2 },
            new int[] { 3, 2, 6 },
            new int[] { 3, 6, 8 },
            new int[] { 3, 8, 9 },
            new int[] { 4, 9, 5 },
            new int[] { 2, 4, 11 },
            new int[] { 6, 2, 10 },
            new int[] { 8, 6, 7 },
            new int[] { 9, 8, 1 }
        };
        foreach (var triangle in triangles)
        {
            Vector3 v0 = vertices[triangle[0]];
            Vector3 v1 = vertices[triangle[1]];
            Vector3 v2 = vertices[triangle[2]];
            Vector3 center = (v0 + v1 + v2) / 3.0f;
            Quaternion rotation = Quaternion.LookRotation((v1 - v0).normalized, Vector3.Cross(v1 - v0, v2 - v0).normalized);
            GameObject prism = Instantiate(prismPrefab, center, rotation);
            prism.transform.localScale = new Vector3(prismScaleFactor, prismScaleFactor, prismScaleFactor);
        }
    }
}
