using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IcosahedronBuilder : MonoBehaviour
{
    [SerializeField] private List<GameObject> faces; 
    [SerializeField] private GameObject prismPrefab; 
    [SerializeField] private GameObject facePrefab;
    [SerializeField] private int iter;
    [SerializeField] private float radiusIco; 
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


        StartCoroutine(GenerateIcosphere(verticesIcosahedron, radiusIco, iter));
    }

    public IEnumerator GenerateIcosphere(Vector3[] vertices, float radiusIco, int iterations)
    {
        Vector3[] combined = vertices;

        float[] distances = GenerateDistances(iterations+1);

        foreach (var vertice in vertices)
        {
            GameObject sphere = Instantiate(prismPrefab, vertice, Quaternion.identity);
            sphere.transform.SetParent(gameObject.transform);
        }

        Debug.Log($"Iteration {0}: {combined.Length} vertices");

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
            Debug.Log($"Iteration {i + 1}: {combined.Length} vertices with distance {distances[i]}");

        }
        Debug.Log(distances[iterations]);
        GenerateFaces(combined, distances[iterations]);

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
            distances[i] = currentDistance * 1.25f;
            currentDistance /= 2f;

        }

        return distances;
    }

    public void GenerateFaces(Vector3[] vertices, float maxDistance)
    {

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

                        GameObject face = Instantiate(facePrefab);

                        GameObject shadow = face.GetComponentInChildren<Shadow>().gameObject;

                        Vector3 normal = Vector3.Cross(vertices[j] - vertices[i], vertices[k] - vertices[i]).normalized;
                        face.transform.position = center;

                        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
                        face.transform.rotation = rotation;

                        
                        float distance = Vector3.Distance(shadow.transform.position, Vector3.zero); // Расстояние до (0,0,0)
                        Debug.Log(distance);
                        if (Mathf.Abs(distance - 0.76f) < 0.01f)
                        {
                            face.transform.Rotate(0, 0, 180, Space.Self);
                        }

                        faces.Add(face);
                    }
                }
            }
        }
        GroupGameObjects(faces.ToArray());
    }

    public void GroupGameObjects(GameObject[] gameObjects)
    {
        List<List<GameObject>> stripes = new List<List<GameObject>>();

        int groupCount = 0;
        if (gameObjects.Length == 20)
        {
            groupCount = 4;
        }
        else if (gameObjects.Length == 80)
        {
            groupCount = 7;
        }
        else if (gameObjects.Length == 320)
        {
            groupCount = 13;
        }
        else
        {
            Debug.Log("ERRORORORORORORO");
        }

        List<GameObject> sortedObjects = new List<GameObject>(faces);
        sortedObjects.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        foreach (var obj in sortedObjects)
        {
            bool added = false;
            foreach (var stripe in stripes)
            {
                if (Mathf.Abs(stripe[0].transform.position.y - obj.transform.position.y) <= 0.25f)
                {
                    stripe.Add(obj);
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                stripes.Add(new List<GameObject> { obj });
            }
        }

        int k = 0;
        foreach (var stripe in stripes)
        {
            char letter = (char)('A' + k);
            GameObject emptyObject = new GameObject("Strip" + letter);

            foreach (var face in stripe)
            {
                face.transform.SetParent(emptyObject.transform);

                if (k % 2 != 0) 
                {
                    AlignLocalZDown(face);
                }
                else
                {
                    AlignLocalZUp(face);
                }
            }
            k++;
        }
    }

    private void AlignLocalZDown(GameObject face)
    {
        Vector3 globalDown = Vector3.down;
        Vector3 projectedDown = Vector3.ProjectOnPlane(globalDown, face.transform.up);

        if (projectedDown.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedDown, face.transform.up);
            face.transform.rotation = targetRotation;
        }
    }

    private void AlignLocalZUp(GameObject face)
    {

        Vector3 globalUp = Vector3.up;
        Vector3 projectedUp = Vector3.ProjectOnPlane(globalUp, face.transform.up);

        if (projectedUp.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedUp, face.transform.up);
            face.transform.rotation = targetRotation;
        }
    }
}