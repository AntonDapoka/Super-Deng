using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class FaceIcosahedronBuilderScript : MonoBehaviour, IBuilderScript
{
    [SerializeField] private List<GameObject> faces;
    [SerializeField] private GameObject prismPrefab;
    [SerializeField] private GameObject facePrefab;

    [SerializeField] protected float radiusIco;
    public float sideLength;
    protected float epsilon = 0.001f;
    [SerializeField] protected bool isTest = false;
    //[SerializeField] private float prismScaleFactor = 0.9f;

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

        StartCoroutine(GenerateIcosahedron(verticesIcosahedron, sideLen));//Change to void after completion
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

    public IEnumerator GenerateIcosahedron(Vector3[] vertices, float radiusIco) //Change to void after completion
    {
        float distance = radiusIco;

        List<GameObject> gameObjects = new List<GameObject>();

        if (isTest)
        {
            foreach (var vertice in vertices)
            {
                GameObject sphere = Instantiate(prismPrefab, vertice, Quaternion.identity);
                sphere.transform.SetParent(gameObject.transform);
                gameObjects.Add(sphere);
                yield return new WaitForSeconds(0.1f);
            }
        }

        //GroupGameObjects(gameObjects.ToArray());



        yield return new WaitForSeconds(1f);
        GenerateFaces(vertices, distance);
    }

    public void GenerateFaces(Vector3[] vertices, float maxDistance)
    {
        int count = 0;
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
                        count++;
                        Vector3 center = (a + b + c) / 3f;

                        GameObject face = Instantiate(facePrefab);
                        face.transform.position = center;



                        Vector3 normal = Vector3.Cross(b - a, c - a).normalized;
                        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
                        face.transform.rotation = rotation;


                        

                        float distanceFace = Vector3.Distance(face.transform.position, Vector3.zero); // Расстояние до (0,0,0)
                        GameObject shadow = face.GetComponent<FaceScript>().shadow;
                        float distanceShadow = Vector3.Distance(shadow.transform.position, Vector3.zero); // Расстояние до (0,0,0)
                        //Debug.Log(distanceFace, distanceShadow);
                        if (distanceFace < distanceShadow) //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        {
                            face.transform.Rotate(0, 0, 180, Space.Self);
                            Debug.Log("ROTATED");
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

                        faces.Add(face);
                        
                    }
                }
            }
        }
        //GroupGameObjects(faces.ToArray());
    }


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