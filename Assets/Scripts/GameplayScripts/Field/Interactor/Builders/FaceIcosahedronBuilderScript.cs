using System.Collections.Generic;
using UnityEngine;

public class FaceIcosahedronBuilderScript : MonoBehaviour, IBuilderScript
{
    [SerializeField] protected bool isTest = false;

    [SerializeField] protected List<GameObject> faces;
    protected GameObject fieldHolder;
    protected GameObject facePrefab;
    [SerializeField] protected GameObject verticePrefab;

    protected float sideLength;
    protected float faceScale;
    protected float radiusIco; //Radius of Icosahedorn

    protected float epsilon = 0.001f;

    public GameObject Holder => fieldHolder;

    public virtual void BuildField(GameObject newFacePrefab, float newSideLength, float newFaceScale) //interface
    {
        //dataStructure.GetData unique class data blablabla
        facePrefab = newFacePrefab;
        sideLength = newSideLength;
        faceScale = newFaceScale;
        BuildIcosahedron();
    }

    protected void BuildIcosahedron()
    {
        radiusIco = sideLength * 0.250000f * Mathf.Sqrt(2.00000f * (5.00000f + Mathf.Sqrt(5.00000f)));
        float radiusPenta = sideLength * (Mathf.Sqrt(10.00000f) * Mathf.Sqrt(5.00000f + Mathf.Sqrt(5.00000f))) / 10.00000f;

        Vector3[] verticesIcosahedron = GetIcosahedronVertices(radiusIco, radiusPenta);

        if (isTest) 
            GenerateInitialVertices(verticesIcosahedron);

        fieldHolder = new GameObject("FieldHolder");
        fieldHolder.transform.position = Vector3.zero;

        GenerateFaces(verticesIcosahedron, sideLength, radiusIco, fieldHolder.transform);
    }

    protected Vector3[] GetIcosahedronVertices(float radiusIco, float radiusPenta)
    {
        Vector3[] vertices = new Vector3[]
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
        return vertices;
    }

    protected void GenerateInitialVertices(Vector3[] vertices)
    {
        GameObject verticeHolder = new GameObject("VerticeHolder");
        verticeHolder.transform.position = Vector3.zero;

        foreach (var vertice in vertices)
        {
            Instantiate(verticePrefab, vertice, Quaternion.identity, verticeHolder.transform);
        }
    }

    protected readonly IFaceIdProviderScript _faceIdProvider = new FaceIdProviderScript();

    protected void GenerateFaces(Vector3[] vertices, float maxDistance, float radius, Transform parent)
    {
        var entries = new List<(GameObject face, long rawKey)>();
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
                    //Debug.Log(abDistance.ToString() + " " + maxDistance);
                    if (Mathf.Abs(maxDistance - abDistance) <= epsilon &&
                        Mathf.Abs(maxDistance - bcDistance) <= epsilon &&
                        Mathf.Abs(maxDistance - acDistance) <= epsilon)
                    {

                        Vector3[] verticesABC = AdjustVerticesToRadius(new Vector3[3] { a, b, c }, radius);
                        long key = _faceIdProvider.ComputeIcosphereFaceKey(i, j, k);
                        GameObject face = SetFace(verticesABC, parent);

                        faces.Add(face);
                        entries.Add((face, key));
                    }
                }
            }
        }
        FaceIdCanonicalizerScript.AssignIds(entries);
        //GroupGameObjects(faces.ToArray());
    }

    protected virtual GameObject SetFace(Vector3[] verticesABC, Transform parent)
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

        return face;
    }

    protected Quaternion SetFaceRightRotation(Vector3 vertexOnZAxis, Vector3[] verticesABC, Vector3 centerTriangle, Vector3 zero)
    {
        Vector3 A = verticesABC[0];
        Vector3 B = verticesABC[1];
        Vector3 C = verticesABC[2];

        Vector3 normal = Vector3.Cross(B - A, C - A).normalized;

        Vector3 forward = (centerTriangle - vertexOnZAxis).normalized;
        Quaternion rotation = Quaternion.LookRotation(forward, normal);

        return rotation;
    }

    protected static Vector3[] AdjustVerticesToRadius(Vector3[] vertices, float radius)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].normalized * radius;
        }
        return vertices;
    }

    public GameObject[] GetField()
    {
        return faces.ToArray();
    }
}