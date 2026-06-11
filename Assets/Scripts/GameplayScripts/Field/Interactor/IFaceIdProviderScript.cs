public interface IFaceIdProviderScript
{
    long ComputeIcosphereFaceKey(int vertexIndexA, int vertexIndexB, int vertexIndexC);
    long ComputeGridFaceKey(int x, int y, int gridWidth);
}
