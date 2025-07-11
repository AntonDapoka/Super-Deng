using UnityEngine;

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(float x, float y, float z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public SerializableVector3(Vector3 v)
    {
        x = v.x; y = v.y; z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}
