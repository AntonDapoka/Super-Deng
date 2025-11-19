using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceIcosahedronBuilderScript : MonoBehaviour, IBuilderScript
{
    [SerializeField] protected bool isTest = false;

    //[SerializeField] private List<GameObject> faces;
    [SerializeField] private GameObject facePrefab;
    [SerializeField] private GameObject verticePrefab;

    [SerializeField] protected float sideLength = 1;
    [SerializeField] protected float radiusIco; //Radius of Icosahedorn

    protected float epsilon = 0.001f;

    private void Start()
    {
        BuildField();
    }

    public void BuildField() //interface
    {
        //dataStructure.GetData unique class data blablabla
        //sideLength = x 

        BuildIcosahedron(sideLength);
    }

    protected void BuildIcosahedron(float sideLen)
    {
        radiusIco = sideLen * 0.250000f * (Mathf.Sqrt(2.00000f * (5.0f + Mathf.Sqrt(5.00000f))));
        float radiusPenta = sideLen * (Mathf.Sqrt(10.00000f) * Mathf.Sqrt(5.0f + Mathf.Sqrt(5.00000f))) / 10.00000f;

        Vector3[] verticesIcosahedron = GetIcosahedronVertices(radiusIco, radiusPenta);


        //if (isTest) 
        GenerateInitialVerticies(verticesIcosahedron);

        //GenerateFaces(verticesIcosahedron, radiusIco);
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

    protected void GenerateInitialVerticies(Vector3[] vertices)
    {
        GameObject verticeHolder = new GameObject("VerticeHolder");
        verticeHolder.transform.position = Vector3.zero;

        foreach (var vertice in vertices)
        {
            Instantiate(verticePrefab, vertice, Quaternion.identity, verticeHolder.transform);
        }
    }

    protected void GenerateFaces(Vector3[] vertices, float maxDistance)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                for (int k = j + 1; k < vertices.Length; k++)
                {
                    Vector3 a = vertices[i];    
                    Vector3 b = vertices[j];
                    Vector3 c = vertices[k];

                    if (Mathf.Abs(maxDistance - Vector3.Distance(a, b)) <= epsilon &&
                        Mathf.Abs(maxDistance - Vector3.Distance(b, c)) <= epsilon &&
                        Mathf.Abs(maxDistance - Vector3.Distance(c, a)) <= epsilon)
                    {
                        Vector3 center = (a + b + c) / 3f;
                        Vector3 normal = Vector3.Cross(b - a, c - a).normalized;
                        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

                        GameObject face = Instantiate(facePrefab, center, rotation);
                        GameObject shadow = face.GetComponent<FaceScript>().shadow;

                        float distanceFace = Vector3.Distance(face.transform.position, Vector3.zero); // Distance to (0,0,0)
                        float distanceShadow = Vector3.Distance(shadow.transform.position, Vector3.zero); // Distance to (0,0,0)

                        if (distanceFace < distanceShadow) //If shadow is futher away from the center, we need to Rotate our Face
                        {
                            face.transform.Rotate(0, 0, 180, Space.Self);
                        }

                        bool AB = Mathf.Abs(a.y - b.y) < epsilon;
                        bool AC = Mathf.Abs(a.y - c.y) < epsilon;
                        bool BC = Mathf.Abs(b.y - c.y) < epsilon;

                        void Align(bool isHigher)
                        {
                            if (isHigher) AlignLocalZDown(face);
                            else AlignLocalZUp(face);
                        }

                        if (AB)
                        {
                            Align(c.y > a.y);
                        }
                        else if (AC)
                        {
                            Align(b.y > a.y);
                        }
                        else if (BC)
                        {
                            Align(a.y > b.y);
                        }

                        //faces.Add(face);
                    }
                }
            }
        }
        //GroupGameObjects(faces.ToArray());
    }

    protected void AlignLocalZDown(GameObject face)
    {
        Vector3 globalDown = Vector3.down;
        Vector3 projectedDown = Vector3.ProjectOnPlane(globalDown, face.transform.up);

        if (projectedDown.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedDown, face.transform.up);
            face.transform.rotation = targetRotation;
        }
    }

    protected void AlignLocalZUp(GameObject face)
    {
        Vector3 globalUp = Vector3.up;
        Vector3 projectedUp = Vector3.ProjectOnPlane(globalUp, face.transform.up);

        if (projectedUp.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedUp, face.transform.up);
            face.transform.rotation = targetRotation;
        }
    }

    /*
    public void GroupGameObjects(GameObject[] gameObjects)
    {
        List<List<GameObject>> stripes = new List<List<GameObject>>();

        List<GameObject> sortedObjects = new List<GameObject>(faces);
        sortedObjects.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        foreach (var obj in sortedObjects)
        {
            bool added = false;
            foreach (var stripe in stripes)
            {
                if (Mathf.Abs(stripe[0].transform.position.y - obj.transform.position.y) <= 0.1f) //////////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                    //face.transform.localScale = Vector3.one / 3f;  
                }
                else
                {
                    AlignLocalZUp(face);
                }
            }
            k++;
        }
    }*/
}