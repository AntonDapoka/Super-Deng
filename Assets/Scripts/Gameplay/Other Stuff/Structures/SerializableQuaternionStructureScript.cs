using UnityEngine;

[System.Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public SerializableQuaternion(float x, float y, float z, float w)
    {
        this.x = x; this.y = y; this.z = z; this.w = w;
    }

    public SerializableQuaternion(Quaternion q)
    {
        x = q.x; y = q.y; z = q.z; w = q.w;
    }

    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
}