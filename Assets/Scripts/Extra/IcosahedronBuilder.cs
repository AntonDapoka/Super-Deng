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
        float a = 1.0f;
        float radiusIco = a * 0.25f * (Mathf.Sqrt(2.0f * (5.0f + Mathf.Sqrt(5.0f))));
        float radiusPenta = a * (Mathf.Sqrt(10.0f) * Mathf.Sqrt(5.0f + Mathf.Sqrt(5.0f))) / 10.0f;

        Vector3 vertex1 = new Vector3(0, radiusIco, 0);
        Vector3 vertex2 = new Vector3(-radiusPenta, radiusPenta / 2, 0);
        Vector3 vertex3 = new Vector3(-Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta);
        Vector3 vertex4 = new Vector3(Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta);
        Vector3 vertex5 = new Vector3(Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, -Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta);
        Vector3 vertex6 = new Vector3(-Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, radiusPenta / 2, -Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta);

        Vector3 vertex7 = new Vector3(radiusPenta, -radiusPenta / 2, 0);
        Vector3 vertex8 = new Vector3(Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, -Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta);
        Vector3 vertex9 = new Vector3(-Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, -Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta);
        Vector3 vertex10 = new Vector3(-Mathf.Cos(36f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, Mathf.Cos(54f * Mathf.Deg2Rad) * radiusPenta);
        Vector3 vertex11 = new Vector3(Mathf.Cos(72f * Mathf.Deg2Rad) * radiusPenta, -radiusPenta / 2, Mathf.Cos(18f * Mathf.Deg2Rad) * radiusPenta);
        Vector3 vertex12 = new Vector3(0, -radiusIco, 0);

        Vector3 center123 = (vertex1 + vertex2 + vertex3) / 3;
        Vector3 center134 = (vertex1 + vertex3 + vertex4) / 3;
        Vector3 center145 = (vertex1 + vertex4 + vertex5) / 3;
        Vector3 center156 = (vertex1 + vertex5 + vertex6) / 3;
        Vector3 center162 = (vertex1 + vertex6 + vertex2) / 3;

        Vector3 center1023 = (vertex10 + vertex2 + vertex3) / 3;
        Vector3 center1134 = (vertex11 + vertex3 + vertex4) / 3;
        Vector3 center745 = (vertex7 + vertex4 + vertex5) / 3; 
        Vector3 center856 = (vertex8 + vertex5 + vertex6) / 3;
        Vector3 center962 = (vertex9 + vertex6 + vertex2) / 3;

        Vector3 center578 = (vertex5 + vertex7 + vertex8) / 3;
        Vector3 center689 = (vertex6 + vertex8 + vertex9) / 3;
        Vector3 center2910 = (vertex2 + vertex9 + vertex10) / 3;
        Vector3 center31011 = (vertex3 + vertex10 + vertex11) / 3;
        Vector3 center4117 = (vertex4 + vertex11 + vertex7) / 3;

        Vector3 center1278 = (vertex12 + vertex7 + vertex8) / 3;
        Vector3 center1289 = (vertex12 + vertex8 + vertex9) / 3;
        Vector3 center12910 = (vertex12 + vertex9 + vertex10) / 3;
        Vector3 center121011 = (vertex12 + vertex10 + vertex11) / 3;
        Vector3 center12117 = (vertex12 + vertex11 + vertex7) / 3;

        Vector3[] vertices = new Vector3[]
        {
            vertex1,
            vertex2,
            vertex3,
            vertex4,
            vertex5,
            vertex6,
            vertex7,
            vertex8,
            vertex9,
            vertex10,
            vertex11,
            vertex12,
            center123,
            center134,
            center145,
            center156,
            center162,

            center1023,
            center1134,
            center745,
            center856,
            center962,

            center578,
             center689,
            center2910,
            center31011,
             center4117,

            center1278,
            center1289,
            center12910,
            center121011,
            center12117
        };

        foreach (var vertice in vertices)
        {
            Instantiate(prismPrefab, vertice, Quaternion.identity);
        }
    }
}
