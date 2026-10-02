using UnityEngine;

public class FaceIdProviderScript : IFaceIdProviderScript
{
    public long ComputeIcosphereFaceKey(int a, int b, int c)
    {
        int min = Mathf.Min(a, Mathf.Min(b, c));
        int max = Mathf.Max(a, Mathf.Max(b, c));
        int mid = a + b + c - min - max;
        return ((long)min << 40) | ((long)mid << 20) | (long)max;
    }

    public long ComputeGridFaceKey(int x, int y, int gridWidth)
    {
        return ((long)y << 32) | (uint)x;
    }
}
